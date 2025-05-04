using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Pets.Commands.ChooseMainPhoto
{
    public record ChoosePetMainPhotoCommand(Guid VolunteerId, Guid PetId, string Path) : ICommand;
}