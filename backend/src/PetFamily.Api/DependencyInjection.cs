using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure;

namespace PetFamily.Web
{
    public static class DependencyInjection
    { 
        public static IServiceCollection AddVolunteersModule(this IServiceCollection services) 
        {
            services
                .AddVolunteersApplication()
                .AddVolunteersInfrastructure();

            return services;    
        }
    }
}
