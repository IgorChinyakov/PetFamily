using CSharpFunctionalExtensions;
using PetFamily.Files.Contracts.DTOs;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Files.Contracts
{
    public interface IFilesContract
    {
        Task<Result<IReadOnlyList<string>, Error>> UploadFiles(IEnumerable<FileData> filesData, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> RemoveFile(FileMeta fileMeta, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> PresignedGetObject(FileMeta fileMeta, CancellationToken cancellationToken = default);

        Task AddToMessageQueue(
            IEnumerable<FileMeta> fileMetas, CancellationToken cancellationToken = default);
    }
}
