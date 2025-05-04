using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Options;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Volunteers.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddVolunteersInfrastructure(
            this IServiceCollection services)
        {
            services
                .AddRepositories()
                .AddDbContexts()
                .AddUnitOfWork()
                .AddBackgroundServices();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IVolunteersRepository, VolunteersRepository>();

            return services;
        }

        private static IServiceCollection AddDbContexts(this IServiceCollection services)
        {
            services.AddScoped<VolunteersWriteDbContext>();
            services.AddScoped<IVolunteersReadDbContext, VolunteersReadDbContext>();

            return services;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(UnitOfWorkKeys.Volunteers);

            return services;
        }

        private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<DeletedVolunteersCleanerBackgroundService>();

            return services;
        }
    }
}
