using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;

namespace PetFamily.Application.Providers
{
    public interface IFilesProvider
    {
        Task<Result<IReadOnlyList<string>, Error>> UploadFiles(IEnumerable<FileData> filesData, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> RemoveFile(FileMeta fileMeta, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> PresignedGetObject(FileMeta fileMeta, CancellationToken cancellationToken = default);
    }
}
