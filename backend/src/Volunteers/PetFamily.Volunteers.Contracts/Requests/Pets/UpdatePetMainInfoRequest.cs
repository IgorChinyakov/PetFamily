using PetFamily.Core.DTOs;

namespace PetFamily.Volunteers.Contracts.Requests.Pets
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
        DateTime CreationDate);
}
