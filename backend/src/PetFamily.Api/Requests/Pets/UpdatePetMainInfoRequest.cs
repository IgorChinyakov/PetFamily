using PetFamily.Application.DTOs;

namespace PetFamily.Api.Requests.Pets
{
    public record UpdatePetMainInfoRequest(
        Guid Id,
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
        bool IsVaccinated)
    {
        public UpdatePetMainInfoCommand ToCommand()
            => new UpdatePetMainInfoCommand(
                Id,
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
                IsVaccinated);
    }
}
