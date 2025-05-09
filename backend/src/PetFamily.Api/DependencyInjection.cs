using PetFamily.Volunteers.Application;
using PetFamily.Specieses.Application;
using PetFamily.Volunteers.Infrastructure;
using PetFamily.Files.Infrastructure;
using PetFamily.Specieses.Infrastructure;
using PetFamily.Volunteers.Presentation;
using PetFamily.Specieses.Presentation;
using PetFamily.Files.Presentation;

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
    }
}
