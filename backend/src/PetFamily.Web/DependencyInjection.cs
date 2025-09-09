using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Infrastructure.Consumers;
using PetFamily.Accounts.Presentation;
using PetFamily.Core.Options;
using PetFamily.Discussions.Application;
using PetFamily.Discussions.Infrastructure;
using PetFamily.Discussions.Infrastructure.Consumers;
using PetFamily.Discussions.Presentation;
using PetFamily.Files.Infrastructure;
using PetFamily.Files.Presentation;
using PetFamily.Specieses.Application;
using PetFamily.Specieses.Infrastructure;
using PetFamily.Specieses.Presentation;
using PetFamily.VolunteerRequests.Application;
using PetFamily.VolunteerRequests.Infrastructure;
using PetFamily.VolunteerRequests.Infrastructure.Consumers;
using PetFamily.VolunteerRequests.Presentation;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure;
using PetFamily.Volunteers.Presentation;

namespace PetFamily.Web
{
    public static class DependencyInjection
    { 
        public static IServiceCollection AddVolunteersModule(
            this IServiceCollection services, IConfiguration configuration) 
        {
            services
                .AddVolunteersApplication()
                .AddVolunteersInfrastructure(configuration)
                .AddVolunteersContracts();

            return services;    
        }

        public static IServiceCollection AddSpeciesModule(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSpeciesApplication()
                .AddSpeciesInfrastructure(configuration)
                .AddSpeciesContracts();

            return services;
        }

        public static IServiceCollection AddFilesModule(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddFilesInfrastructure(configuration)
                .AddFilesContracts();

            return services;
        }

        public static IServiceCollection AddAccountsModule(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAccountsInfrastructure(configuration)
                .AddAccountsApplication()
                .AddAccountsContracts();

            return services;
        }

        public static IServiceCollection AddVolunteerRequestsModule(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddVolunteerRequestsInfrastructure(configuration)
                .AddVolunteerRequestsApplication()
                .AddVolunteerRequestsContracts();

            return services;
        }

        public static IServiceCollection AddDiscussionsModule(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDiscussionsInfrastructure(configuration)
                .AddDiscussionsApplication()
                .AddDiscussionsContracts();

            return services;
        }

        public static IServiceCollection AddMessageBus(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = configuration.GetSection(RabbitMQOptions.RABBIT_MQ).Get<RabbitMQOptions>()!;

            services.AddMassTransit(conf =>
            {
                conf.AddConsumer<RequestTakenOnReview_CreateDiscussionConsumer>();
                conf.AddConsumer<DiscussionCreated_UpdateRequestConsumer>();
                conf.AddConsumer<RequestApproved_CreateVolunteerAccountConsumer>();

                conf.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(options.Host), h =>
                    {
                        h.Username(options.UserName);
                        h.Password(options.Password);
                    });

                    cfg.Durable = true;

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}