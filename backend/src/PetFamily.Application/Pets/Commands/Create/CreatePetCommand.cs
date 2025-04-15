using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Application.Pets.Commands.Create
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
        Status PetStatus) : ICommand;
}
