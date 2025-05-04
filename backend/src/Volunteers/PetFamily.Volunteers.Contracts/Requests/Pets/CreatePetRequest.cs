using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Application.Pets.Commands.Create;
using PetFamily.Volunteers.Domain.PetsVO;

namespace PetFamily.Volunteers.Contracts.Requests.Pets
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
