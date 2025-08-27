using CSharpFunctionalExtensions;
using DotNet.Testcontainers.Builders;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using NSubstitute;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Infrastructure.Authorization.Seeding;
using PetFamily.Accounts.Infrastructure.Consumers;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Infrastructure.Consumers;
using PetFamily.Discussions.Infrastructure.DbContexts;
using PetFamily.Files.Application;
using PetFamily.Files.Contracts;
using PetFamily.Files.Contracts.DTOs;
using PetFamily.Files.Infrastructure.BackgroundServices;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application.Database;
using PetFamily.Specieses.Infrastructure.BackgroundServices;
using PetFamily.Specieses.Infrastructure.DbContexts;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Infrastructure.Consumers;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Infrastructure.BackgroundServices;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using Respawn;
using System.ComponentModel;
using System.Data.Common;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using static MassTransit.Logging.DiagnosticHeaders.Messaging;

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

        private readonly RabbitMqContainer _rabbitMQContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3.13.7-management")
            .WithUsername("test")
            .WithPassword("rabbitmqtest")
            .WithEnvironment("RABBITMQ_DEFAULT_USER", "test")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
            .Build();

        private Respawner _respawner = default!;
        private DbConnection _dbConnection = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureDefaultServices);
            builder.UseEnvironment("Testing");
        }

        protected virtual void ConfigureDefaultServices(IServiceCollection services)
        {
            //rabbit
            var descriptors = services
                .Where(d => d.ImplementationType?.Name?.Contains("MassTransit") == true
                    || d.ServiceType.FullName?.Contains("MassTransit") == true)
                .ToList();

            foreach (var d in descriptors)
                services.Remove(d);
            //contexts
            var volunteersWriteContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(VolunteersWriteDbContext));

            var volunteerRequestsWriteContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(VolunteerRequestsWriteDbContext));

            var discussionsDbContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(DiscussionsWriteDbContext));

            var accountDbContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(AccountDbContext));

            var speciesWriteContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(SpeciesWriteDbContext));

            var volunteersReadContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(ISpeciesReadDbContext));

            var speciesReadContext = services.SingleOrDefault(s =>
               s.ServiceType == typeof(IVolunteersReadDbContext));

            var volunteerRequestsReadContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IVolunteerRequestsReadDbContext));

            var discussionsReadContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IDiscussionsReadDbContext));

            //background jobs
            var accountSeeder = services.SingleOrDefault(s =>
                s.ServiceType == typeof(AccountsSeeder));

            var volunteersCleanerService = services.SingleOrDefault(s =>
                s.ImplementationType == typeof(DeletedVolunteersCleanerBackgroundService));

            var speciesCleanerService = services.SingleOrDefault(s =>
                s.ImplementationType == typeof(DeletedSpeciesCleanerBackgroundService));

            var filesCleanerService = services.SingleOrDefault(s =>
                s.ImplementationType == typeof(FilesCleanerBackgroundService));

            var filesContract = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IFilesContract));

            //removemnt
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

            if (accountSeeder is not null)
                services.Remove(accountSeeder);

            if (accountDbContext is not null)
                services.Remove(accountDbContext);

            if (volunteerRequestsWriteContext is not null)
                services.Remove(volunteerRequestsWriteContext);

            if (discussionsDbContext is not null)
                services.Remove(discussionsDbContext);

            if (speciesWriteContext is not null)
                services.Remove(speciesWriteContext);

            if (volunteersReadContext is not null)
                services.Remove(volunteersReadContext);

            if (speciesReadContext is not null)
                services.Remove(speciesReadContext);

            if (discussionsReadContext is not null)
                services.Remove(discussionsReadContext);

            if (volunteerRequestsReadContext is not null)
                services.Remove(volunteerRequestsReadContext);

            //addition
            services.AddSingleton(_ => _filesProviderMock);

            services.AddSingleton<AccountsSeeder>();

            services.AddScoped(_ =>
                new VolunteersWriteDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped(_ =>
                new VolunteerRequestsWriteDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped(_ =>
                new DiscussionsWriteDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped(_ =>
                new AccountDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped(_ =>
                new SpeciesWriteDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped<IVolunteersReadDbContext>(_ =>
                new VolunteersReadDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped<ISpeciesReadDbContext>(_ =>
                new SpeciesReadDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped<IVolunteerRequestsReadDbContext>(_ =>
                new VolunteerRequestsReadDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped<IDiscussionsReadDbContext>(_ =>
                new DiscussionsReadDbContext(_dbContainer.GetConnectionString()));

            services.AddMassTransit(conf =>
            {
                conf.AddConsumer<RequestTakenOnReview_CreateDiscussionConsumer>();
                conf.AddConsumer<DiscussionCreated_UpdateRequestConsumer>();
                conf.AddConsumer<RequestApproved_CreateVolunteerAccountConsumer>();

                conf.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri($"amqp://test:rabbitmqtest@localhost:{_rabbitMQContainer.GetMappedPublicPort(5672)}"), h =>
                    {
                        h.Username("test");
                        h.Password("rabbitmqtest");
                    });

                    cfg.Durable = true;

                    cfg.ConfigureEndpoints(context);
                });
            });
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
            await _rabbitMQContainer.StopAsync();
            await _dbContainer.DisposeAsync();
            await _rabbitMQContainer.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            await _rabbitMQContainer.StartAsync();

            using var scope = Services.CreateScope();

            var volunteerRequestsWriteDbContext = scope.ServiceProvider.GetRequiredService<VolunteerRequestsWriteDbContext>();
            await volunteerRequestsWriteDbContext.Database.MigrateAsync();

            var volunteersWriteDbContext = scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
            await volunteersWriteDbContext.Database.MigrateAsync();

            var discussionsWriteDbContext = scope.ServiceProvider.GetRequiredService<DiscussionsWriteDbContext>();
            await discussionsWriteDbContext.Database.MigrateAsync();

            var speciesWriteDbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
            await speciesWriteDbContext.Database.MigrateAsync();

            var accountDbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
            await accountDbContext.Database.MigrateAsync();

            var seeder = scope.ServiceProvider.GetRequiredService<AccountsSeeder>();
            await seeder.SeedAsync();

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
