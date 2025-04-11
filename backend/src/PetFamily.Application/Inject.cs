using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Pets.UseCases.Create;
using PetFamily.Application.Pets.UseCases.Move;
using PetFamily.Application.Pets.UseCases.UploadPhotos;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.UseCases.Create;
using PetFamily.Application.Volunteers.UseCases.Delete;
using PetFamily.Application.Volunteers.UseCases.UpdateDetails;
using PetFamily.Application.Volunteers.UseCases.UpdateMainInfo;
using PetFamily.Application.Volunteers.UseCases.UpdateSocialMedia;

namespace PetFamily.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreatePetHandler>();
            services.AddScoped<MovePetHandler>();
            services.AddScoped<CreateVolunteerHandler>();
            services.AddScoped<UpdateMainInfoHandler>();
            services.AddScoped<UpdateSocialMediaHandler>();
            services.AddScoped<UpdateDetailsHandler>();
            services.AddScoped<DeleteVolunteerHandler>();
            services.AddScoped<UploadPhotosHandler>();

            services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

            return services;
        }
    }
}
