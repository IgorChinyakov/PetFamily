using PetFamily.Volunteers.Application.Pets.Commands.UpdateStatus;
using PetFamily.Volunteers.Domain.PetsVO;

namespace PetFamily.Volunteers.Contracts.Requests.Pets
{
    public record UpdatePetStatusRequest(Status Status)
    {
        public UpdatePetStatusCommand ToCommand(Guid petId, Guid volunteerId)
            => new UpdatePetStatusCommand(volunteerId, petId, Status);
    }
}