using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.FileProvider
{
    public record FilesData(IEnumerable<FileContent> Files, string BucketName);

    public record FileContent(Stream Stream, string FileName);
}
