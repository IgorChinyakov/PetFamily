using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Core.FileDtos;
using PetFamily.Files.Application;
using PetFamily.SharedKernel;

namespace PetFamily.Files.Infrastructure.Providers
{
    public class MinioProvider : IFilesProvider
    {
        private const int PRESIGNED_EXPIRATION_TIME = 60 * 60 * 24;
        private const int MAX_COUNT_OF_PARALLELED_PROCESSING_FILES = 5;

        private readonly IMinioClient _minioClient;
        private readonly ILogger<MinioProvider> _logger;

        public MinioProvider(
            IMinioClient minioClient, ILogger<MinioProvider> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyList<string>, Error>> UploadFiles(
            IEnumerable<FileData> filesData,
            CancellationToken cancellationToken = default)
        {
            var semaphor = new SemaphoreSlim(MAX_COUNT_OF_PARALLELED_PROCESSING_FILES);

            var filesList = filesData.ToList();

            try
            {
                await IfBucketNotExistCreateBucket(filesList, cancellationToken);

                var tasks = filesList.Select(async file =>
                    await PutObject(file, semaphor, cancellationToken));

                var pathsResult = await Task.WhenAll(tasks);
                if (pathsResult.Any(p => p.IsFailure))
                    return pathsResult.First().Error;

                var results = pathsResult.Select(p => p.Value).ToList();

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to upload files in minio, files amount: {amount}", filesList.Count);

                return Error.Failure("file.upload", "Fail to upload files in minio");
            }
        }

        public async Task<Result<string, Error>> RemoveFile(
            FileMeta fileMeta,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var statArgs = new StatObjectArgs()
                    .WithBucket(fileMeta.BucketName)
                    .WithObject(fileMeta.FilePath);

                var objectStat = await _minioClient.StatObjectAsync(statArgs, cancellationToken);
                if (objectStat is null)
                    return fileMeta.FilePath;

                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(fileMeta.BucketName)
                    .WithObject(fileMeta.FilePath);

                await _minioClient
                    .RemoveObjectAsync(removeObjectArgs, cancellationToken);

                return fileMeta.FilePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to remove file from minio");
                return Error.Failure("file.deletion", "Fail to remove file from minio");
            }
        }

        public async Task<Result<string, Error>> PresignedGetObject(
            FileMeta fileMeta,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(fileMeta.BucketName)
                    .WithObject(fileMeta.FilePath)
                    .WithExpiry(PRESIGNED_EXPIRATION_TIME);

                var result = await _minioClient
                    .PresignedGetObjectAsync(presignedGetObjectArgs);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to get file from minio");
                return Error.Failure("file.get.persigned", "Fail to get file from minio");
            }
        }

        private async Task<Result<string, Error>> PutObject(
            FileData fileData,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);

            var putObjectArgs = new PutObjectArgs()
             .WithBucket(fileData.BucketName)
             .WithStreamData(fileData.Stream)
             .WithObjectSize(fileData.Stream.Length)
             .WithObject(fileData.FilePath);

            try
            {
                await _minioClient
                    .PutObjectAsync(putObjectArgs, cancellationToken);

                return fileData.FilePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to upload file in minio with path {path} in bucket {bucket}",
                    fileData.FilePath,
                    fileData.BucketName);

                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private async Task IfBucketNotExistCreateBucket(
            IEnumerable<FileData> filesData,
            CancellationToken cancellationToken)
        {
            HashSet<string> bucketNames = filesData.Select(file => file.BucketName).ToHashSet();

            foreach (var name in bucketNames)
            {
                var bucketExistsArgs = new BucketExistsArgs()
                   .WithBucket(name);

                var bucketExists = await _minioClient
                    .BucketExistsAsync(bucketExistsArgs, cancellationToken);

                if (!bucketExists)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(name);

                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }
            }
        }
    }
}
