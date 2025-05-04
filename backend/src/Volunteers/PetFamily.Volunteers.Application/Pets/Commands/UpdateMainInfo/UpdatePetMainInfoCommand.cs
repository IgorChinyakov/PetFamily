using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Volunteers.Application.Pets.Commands.UpdateMainInfo
{
    public record UpdatePetMainInfoCommand(
        Guid PetId,
        Guid VolunteerId,
        Guid SpeciesId,
        Guid BreedId,
        string NickName,
        AddressDto Address,
        string Color,
        string HealthInformation,
        string Description,
        string PhoneNumber,
        float Height,
        float Weight,
        bool IsSterilized,
        bool IsVaccinated,
        DateTime Birthday,
        DateTime CreationDate) : ICommand;
}
