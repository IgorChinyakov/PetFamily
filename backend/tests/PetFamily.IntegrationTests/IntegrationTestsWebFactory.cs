using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NSubstitute;
using PetFamily.Files.Application;
using PetFamily.Files.Contracts;
using PetFamily.Files.Contracts.DTOs;
using PetFamily.Files.Infrastructure.BackgroundServices;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application.Database;
using PetFamily.Specieses.Infrastructure.BackgroundServices;
using PetFamily.Specieses.Infrastructure.DbContexts;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Infrastructure.BackgroundServices;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace PetFamily.IntegrationTests
{
    public class IntegrationTestsWebFactory :
        WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly IFilesContract _filesProviderMock = Substitute.For<IFilesContract>();

        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres")
            .WithDatabase("pet_family_tests")
            .WithUsername("postgres")
            .WithPassword("1234")
            .Build();

        private Respawner _respawner = default!;
        private DbConnection _dbConnection = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureDefaultServices);
        }

        protected virtual void ConfigureDefaultServices(IServiceCollection services)
        {
            var volunteersWriteContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(VolunteersWriteDbContext));

            var speciesWriteContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(SpeciesWriteDbContext));

            var volunteersReadContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(ISpeciesReadDbContext));

            var speciesReadContext = services.SingleOrDefault(s =>
               s.ServiceType == typeof(IVolunteersReadDbContext));

            var volunteersCleanerService = services.SingleOrDefault(s =>
                s.ImplementationType == typeof(DeletedVolunteersCleanerBackgroundService));

            var speciesCleanerService = services.SingleOrDefault(s =>
                s.ImplementationType == typeof(DeletedSpeciesCleanerBackgroundService));

            var filesCleanerService = services.SingleOrDefault(s =>
                s.ImplementationType == typeof(FilesCleanerBackgroundService));

            var filesContract = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IFilesContract));

            if (volunteersCleanerService is not null)
                services.Remove(volunteersCleanerService);

            if (speciesCleanerService is not null)
                services.Remove(speciesCleanerService);

            if (filesContract is not null)
                services.Remove(filesContract);

            if (filesCleanerService is not null)
                services.Remove(filesCleanerService);

            if (volunteersWriteContext is not null)
                services.Remove(volunteersWriteContext);

            if (speciesWriteContext is not null)
                services.Remove(speciesWriteContext);

            if (volunteersReadContext is not null)
                services.Remove(volunteersReadContext);

            if (speciesReadContext is not null)
                services.Remove(speciesReadContext);

            services.AddSingleton(_ => _filesProviderMock);

            services.AddScoped(_ =>
                new VolunteersWriteDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped(_ =>
                new SpeciesWriteDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped<IVolunteersReadDbContext>(_ =>
                new VolunteersReadDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped<ISpeciesReadDbContext>(_ =>
                new SpeciesReadDbContext(_dbContainer.GetConnectionString()));
        }

        public void SetupSuccessFilesProviderMock()
        {
            IReadOnlyList<string> filePaths = new List<string>() { "path" };
            _filesProviderMock.UploadFiles(Arg.Any<IEnumerable<FileData>>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<IReadOnlyList<string>, Error>(filePaths));
        }

        public void SetupFailureFilesProviderMock()
        {
            _filesProviderMock.UploadFiles(Arg.Any<IEnumerable<FileData>>(), Arg.Any<CancellationToken>())
                .Returns(Result.Failure<IReadOnlyList<string>, Error>
                (Error.Failure("failed.to.upload.file", "Failed to upload file")));
        }

        public async Task ResetDatabase()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            using var scope = Services.CreateScope();

            var volunteersWriteDbContext = scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
            await volunteersWriteDbContext.Database.MigrateAsync();

            var speciesWriteDbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
            await speciesWriteDbContext.Database.MigrateAsync();

            _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
            await InitializeRespawner();
        }

        private async Task InitializeRespawner()
        {
            await _dbConnection.OpenAsync();
            _respawner = await Respawner.CreateAsync(
                _dbConnection,
                new RespawnerOptions
                {
                    DbAdapter = DbAdapter.Postgres
                });
        }
    }
}
