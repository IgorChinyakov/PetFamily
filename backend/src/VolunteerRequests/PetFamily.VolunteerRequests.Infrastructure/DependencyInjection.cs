using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddVolunteerRequestsInfrastructure(
           this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContexts(configuration);

            return services;
        }

        private static IServiceCollection AddDbContexts(
           this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<VolunteerRequestsDbContext>(_ =>
                new VolunteerRequestsDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            return services;
        }
    }
}
