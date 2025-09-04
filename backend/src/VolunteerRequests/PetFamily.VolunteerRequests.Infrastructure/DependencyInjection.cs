using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;
using PetFamily.VolunteerRequests.Infrastructure.Outbox;
using PetFamily.VolunteerRequests.Infrastructure.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharpFunctionalExtensions.Result;

namespace PetFamily.VolunteerRequests.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddVolunteerRequestsInfrastructure(
           this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContexts(configuration)
                .AddVolunteerRequestOptions(configuration)
                .AddUnitOfWork()
                .AddQuartzService()
                .AddRepositories();

            return services;
        }

        private static IServiceCollection AddDbContexts(
           this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<VolunteerRequestsWriteDbContext>(_ =>
                new VolunteerRequestsWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            services.AddScoped<IVolunteerRequestsReadDbContext>(_ =>
                new VolunteerRequestsReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            return services;
        }

        private static IServiceCollection AddQuartzService(
           this IServiceCollection services)
        {
            services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessRequestsOutboxMessagesJob));

                configure
                    .AddJob<ProcessRequestsOutboxMessagesJob>(jobKey)
                    .AddTrigger(trigger => trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(1).RepeatForever()));
            });

            services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

            services.AddScoped<ProcessRequestsOutboxMessagesService>();

            return services;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(UnitOfWorkKeys.VolunteerRequests);

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IVolunteerRequestsRepository, VolunteerRequestsRepository>();
            services.AddKeyedScoped<IOutboxRepository, OutboxRepository>(OutboxKeys.VolunteerRequests);

            return services;
        }

        private static IServiceCollection AddVolunteerRequestOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<VolunteerRequestSettings>(configuration.GetSection(VolunteerRequestSettings.Path));

            return services;
        }
    }
}
