using PetFamily.Application.DTOs;
using PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateMainInfo;

namespace PetFamily.Api.Requests.Pets
{
    public record UpdatePetMainInfoRequest(
        Guid SpeciesId,
        Guid BreedId,
        string NickName,
        AddressDto Address,
        string Color,
        string HealthInformation,
        string Description,
        string PhoneNumber,
        int Height,
        int Weight,
        bool IsSterilized,
        bool IsVaccinated,
        DateTime Birthday,
        DateTime CreationDate)
    {
        public UpdatePetMainInfoCommand ToCommand(Guid petId, Guid volunteerId)
            => new UpdatePetMainInfoCommand(
                petId,
                volunteerId,
                SpeciesId,
                BreedId,
                NickName,
                Address,
                Color,
                HealthInformation,
                Description,
                PhoneNumber,
                Height,
                Weight,
                IsSterilized,
                IsVaccinated,
                Birthday,
                CreationDate);
    }
}
