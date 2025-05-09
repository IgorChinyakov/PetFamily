using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Files.Contracts.DTOs
{
    public record FileData(Stream Stream, string FilePath, string BucketName);
}
