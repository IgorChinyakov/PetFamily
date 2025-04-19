using PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateMainInfo;
using PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateStatus;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Api.Requests.Pets
{
    public record UpdatePetStatusRequest(Status Status)
    {
        public UpdatePetStatusCommand ToCommand(Guid petId, Guid volunteerId)
            => new UpdatePetStatusCommand(volunteerId, petId, Status);
    }
}