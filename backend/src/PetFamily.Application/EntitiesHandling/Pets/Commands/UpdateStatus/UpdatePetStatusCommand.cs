using PetFamily.Application.Abstractions;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateStatus
{
    public record UpdatePetStatusCommand(
        Guid VolunteerId, 
        Guid PetId, 
        Status Status) : ICommand;
}