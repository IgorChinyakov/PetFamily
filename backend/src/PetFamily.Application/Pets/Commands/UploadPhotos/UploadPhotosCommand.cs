using PetFamily.Application.Abstractions;
using PetFamily.Application.FileProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Pets.Commands.UploadPhotos
{
    public record UploadPhotosCommand(
        IEnumerable<CreateFileDto> FileDtos, 
        Guid VolunteerId, 
        Guid PetId, 
        string BucketName) : ICommand;
}
