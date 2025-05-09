using CSharpFunctionalExtensions;
using PetFamily.Files.Application;
using PetFamily.Files.Contracts;
using PetFamily.Files.Contracts.DTOs;
using PetFamily.Files.Infrastructure.Messaging;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Files.Presentation
{
    public class FilesContract : IFilesContract
    {
        private readonly IFilesProvider _filesProvider;
        private readonly IMessageQueue<IEnumerable<FileMeta>> _messageQueue;

        public FilesContract(
            IFilesProvider filesProvider, 
            IMessageQueue<IEnumerable<FileMeta>> messageQueue)
        {
            _filesProvider = filesProvider;
            _messageQueue = messageQueue;
        }

        public Task<Result<string, Error>> PresignedGetObject(
            FileMeta fileMeta, CancellationToken cancellationToken = default)
        {
            return _filesProvider.PresignedGetObject(fileMeta, cancellationToken);
        }

        public Task<Result<string, Error>> RemoveFile(
            FileMeta fileMeta, CancellationToken cancellationToken = default)
        {
            return _filesProvider.RemoveFile(fileMeta, cancellationToken);
        }

        public Task<Result<IReadOnlyList<string>, Error>> UploadFiles(
            IEnumerable<FileData> filesData, CancellationToken cancellationToken = default)
        {
            return _filesProvider.UploadFiles(filesData, cancellationToken);
        }

        public async Task AddToMessageQueue(
            IEnumerable<FileMeta> fileMetas, CancellationToken cancellationToken = default)
        => await _messageQueue.WriteAsync(fileMetas, cancellationToken);
    }
}
