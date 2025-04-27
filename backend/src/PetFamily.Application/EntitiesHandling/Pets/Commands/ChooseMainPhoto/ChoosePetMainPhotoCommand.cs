
using PetFamily.Application.Abstractions;

namespace PetFamily.Api.Requests.Pets
{
    public record ChoosePetMainPhotoCommand(Guid VolunteerId, Guid PetId, string Path) : ICommand;
}