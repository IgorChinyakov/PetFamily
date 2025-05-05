using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;

namespace PetFamily.Volunteers.Application.Pets.Commands.UpdateStatus
{
    public record UpdatePetStatusCommand(
        Guid VolunteerId,
        Guid PetId,
        PetStatusDto Status) : ICommand;
}