using PetFamily.Application.Volunteers.DTOs;
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
        Status PetStatus);
}
