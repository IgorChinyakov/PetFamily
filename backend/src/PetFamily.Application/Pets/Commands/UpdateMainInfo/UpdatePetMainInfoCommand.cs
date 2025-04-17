using PetFamily.Application.DTOs;

namespace PetFamily.Api.Requests.Pets
{
    public record UpdatePetMainInfoCommand(
        Guid id, 
        Guid speciesId, 
        Guid breedId, 
        string nickName, 
        AddressDto address,
        string color,
        string healthInformation,
        string Description,
        string PhoneNumber,
        int Height,
        int Weight,
        bool IsSterilized,
        bool IsVaccinated);
}