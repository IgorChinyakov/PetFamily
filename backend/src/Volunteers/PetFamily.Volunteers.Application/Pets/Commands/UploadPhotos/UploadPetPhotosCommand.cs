using PetFamily.Core.Abstractions;
using PetFamily.Core.FileDtos;

namespace PetFamily.Volunteers.Application.Pets.Commands.UploadPhotos
{
    public record UploadPetPhotosCommand(
        IEnumerable<CreateFileDto> FileDtos,
        Guid VolunteerId,
        Guid PetId,
        string BucketName) : ICommand;
}
