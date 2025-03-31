using PetFamily.Application.FileProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Pets.UploadPhotos
{
    public record UploadPhotosCommand(IEnumerable<FileDto> FileDtos, Guid VolunteerId, Guid PetId, string BucketName);
}
