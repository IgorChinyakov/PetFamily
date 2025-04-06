using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Infrastructure.Providers
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

        public async Task<UnitResult<Error>> UploadFiles(
            FilesData filesData,
            CancellationToken cancellationToken = default)
        {
            var semaphor = new SemaphoreSlim(MAX_COUNT_OF_PARALLELED_PROCESSING_FILES);

            try
            {
                var bucketExistsArgs = new BucketExistsArgs()
                   .WithBucket(filesData.BucketName);

                var bucketExists = await _minioClient
                    .BucketExistsAsync(bucketExistsArgs, cancellationToken);

                if (!bucketExists)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(filesData.BucketName);

                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }

                List<Task> tasks = [];
                foreach (var files in filesData.Files)
                {
                    await semaphor.WaitAsync(cancellationToken);

                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(filesData.BucketName)
                        .WithStreamData(files.Stream)
                        .WithObjectSize(files.Stream.Length)
                        .WithObject(files.FileName);

                    var task = _minioClient
                        .PutObjectAsync(putObjectArgs, cancellationToken);

                    semaphor.Release();

                    tasks.Add(task);
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to upload file in minio");
                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
            finally
            {
                semaphor.Release();
            }

            return Result.Success<Error>();
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
    }
}
