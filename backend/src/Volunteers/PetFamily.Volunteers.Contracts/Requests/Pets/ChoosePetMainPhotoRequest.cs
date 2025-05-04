using PetFamily.Volunteers.Application.Pets.Commands.ChooseMainPhoto;

namespace PetFamily.Volunteers.Contracts.Requests.Pets
{
    public record ChoosePetMainPhotoRequest(string Path)
    {
        public ChoosePetMainPhotoCommand ToCommand(Guid volunteerId, Guid petId)
            => new ChoosePetMainPhotoCommand(volunteerId, petId, Path);
    }
}