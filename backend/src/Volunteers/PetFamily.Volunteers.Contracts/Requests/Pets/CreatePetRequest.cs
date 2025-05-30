using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Contracts.DTOs;

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
        PetStatusDto PetStatus,
        List<DetailsDto> Details);
}
