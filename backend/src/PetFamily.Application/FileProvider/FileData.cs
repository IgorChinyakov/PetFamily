using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.FileProvider
{
    public record FileData(Stream Stream, string BucketName, string ObjectName);
}
