using PetFamily.Core.Abstractions;
using PetFamily.Core.FileProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Pets.Commands.UploadPhotos
{
    public record UploadPetPhotosCommand(
        IEnumerable<CreateFileDto> FileDtos,
        Guid VolunteerId,
        Guid PetId,
        string BucketName) : ICommand;
}
