using CSharpFunctionalExtensions;
using PetFamily.Files.Contracts.DTOs;
using PetFamily.SharedKernel;

namespace PetFamily.Files.Application
{
    public interface IFilesProvider
    {
        Task<Result<IReadOnlyList<string>, Error>> UploadFiles(IEnumerable<FileData> filesData, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> RemoveFile(FileMeta fileMeta, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> PresignedGetObject(FileMeta fileMeta, CancellationToken cancellationToken = default);
    }
}
