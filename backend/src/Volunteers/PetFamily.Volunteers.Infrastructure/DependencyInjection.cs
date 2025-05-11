using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Options;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using PetFamily.Volunteers.Infrastructure.Repositories;

namespace PetFamily.Volunteers.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddVolunteersInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddRepositories()
                .AddDbContexts(configuration)
                .AddUnitOfWork()
                .AddBackgroundServices();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IVolunteersRepository, VolunteersRepository>();

            return services;
        }

        private static IServiceCollection AddDbContexts(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<VolunteersWriteDbContext>(_ => 
                new VolunteersWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            services.AddScoped<IVolunteersReadDbContext, VolunteersReadDbContext>(_ =>
                new VolunteersReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            services.AddScoped<ISqlDbConnectionFactory, SqlDbConnectionFactory>();

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
