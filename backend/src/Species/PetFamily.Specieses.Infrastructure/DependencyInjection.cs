using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Options;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.Repositories;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application.Database;
using PetFamily.Volunteers.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Specieses.Infrastructure
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddSpeciesInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddBackgroundServices()
                .AddUnitOfWork()
                .AddDbContexts(configuration)
                .AddRepositories();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();

            return services;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(UnitOfWorkKeys.Species);

            return services;
        }

        private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<DeletedSpeciesCleanerBackgroundService>();

            return services;
        }

        private static IServiceCollection AddDbContexts(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<SpeciesWriteDbContext>(_ =>
                new SpeciesWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            services.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>(_ =>
                new SpeciesReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            return services;
        }
    }
}
