using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Domain.PetsVO;

namespace PetFamily.Volunteers.Application.Pets.Commands.UpdateStatus
{
    public record UpdatePetStatusCommand(
        Guid VolunteerId,
        Guid PetId,
        Status Status) : ICommand;
}