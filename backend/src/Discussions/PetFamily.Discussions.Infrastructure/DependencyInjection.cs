using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Options;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Infrastructure;
using PetFamily.Discussions.Infrastructure.DbContexts;
using PetFamily.Discussions.Infrastructure.Repositories;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequests.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDiscussionsInfrastructure(
           this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContexts(configuration)
                .AddUnitOfWork()
                .AddRepositories();

            return services;
        }

        private static IServiceCollection AddDbContexts(
           this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(_ =>
                new DiscussionsWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            services.AddScoped<IDiscussionsReadDbContext>(_ =>
                new DiscussionsReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            return services;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(UnitOfWorkKeys.Discussions);

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDiscussionsRepository, DiscussionsRepository>();

            return services;
        }
    }
}
