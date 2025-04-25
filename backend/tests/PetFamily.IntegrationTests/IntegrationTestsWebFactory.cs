using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PetFamily.Application.Database;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.DbContexts;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace PetFamily.IntegrationTests
{
    public class IntegrationTestsWebFactory :
        WebApplicationFactory<Program>, IAsyncLifetime
    {
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
            var writeContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(WriteDbContext));

            var readContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IReadDbContext));

            var cleanerService = services.SingleOrDefault(s =>
                s.ImplementationType == typeof(DeletedEntitiesCleanerBackgroundService));

            if (cleanerService is not null)
                services.Remove(cleanerService);

            if (writeContext is not null)
                services.Remove(writeContext);

            if (readContext is not null)
                services.Remove(readContext);

            services.AddScoped(_ =>
                new WriteDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped<IReadDbContext>(_ =>
                new ReadDbContext(_dbContainer.GetConnectionString()));
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
            var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
            await dbContext.Database.EnsureCreatedAsync();

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
