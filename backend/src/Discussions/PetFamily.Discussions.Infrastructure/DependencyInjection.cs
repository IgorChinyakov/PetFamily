using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Discussions.Infrastructure.DbContexts;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequests.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDiscussionsInfrastructure(
           this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContexts(configuration);

            return services;
        }

        private static IServiceCollection AddDbContexts(
           this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DiscussionsDbContext>(_ =>
                new DiscussionsDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            return services;
        }
    }
}
