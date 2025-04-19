using PetFamily.Application.EntitiesHandling.Pets.Commands.Move;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Api.Requests.Pets
{
    public record ChoosePetMainPhotoRequest(string path)
    {
        public ChoosePetMainPhotoCommand ToCommand(Guid volunteerId, Guid petId)
            => new ChoosePetMainPhotoCommand(volunteerId, petId, path);
    }
}