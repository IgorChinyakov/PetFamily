using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Domain.PetsVO;

namespace PetFamily.Volunteers.Application.Pets.Commands.Create
{
    public record CreatePetCommand(
        Guid VolunteerId,
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
        PetStatusDto PetStatus) : ICommand;
}
