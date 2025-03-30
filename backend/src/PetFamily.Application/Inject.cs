using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Pets.Create;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.UpdateDetails;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialMedia;

namespace PetFamily.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreatePetHandler>();
            services.AddScoped<CreateVolunteerHandler>();
            services.AddScoped<UpdateMainInfoHandler>();
            services.AddScoped<UpdateSocialMediaHandler>();
            services.AddScoped<UpdateDetailsHandler>();
            services.AddScoped<DeleteVolunteerHandler>();

            services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

            return services;
        }
    }
}
