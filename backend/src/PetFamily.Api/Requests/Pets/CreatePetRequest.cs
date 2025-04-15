using PetFamily.Application.DTOs;
using PetFamily.Application.Pets.Commands.Create;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Api.Requests.Pets
{
    public record CreatePetRequest(
        string NickName, 
        string Description,
        Guid SpeciesId,
        Guid BreedId,
        string Color,
        bool IsSterilized,
        bool IsVaccinated,
        string HealthInformation,
        AddressDto Address,
        float Weight,
        float Height,
        DateTime Birthday,
        Status PetStatus)
    {
        public CreatePetCommand ToCommand(Guid volunteerId)
            => new CreatePetCommand(
                volunteerId, 
                NickName, 
                Description, 
                SpeciesId, 
                BreedId, 
                Color, 
                IsSterilized, 
                IsVaccinated, 
                HealthInformation, 
                Address, 
                Weight, 
                Height, 
                Birthday, 
                PetStatus);
    }
}
