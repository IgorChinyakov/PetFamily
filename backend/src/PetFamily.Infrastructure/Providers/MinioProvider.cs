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
        private const int PERSIGNED_EXPIRATION_TIME = 60 * 60 * 24;
        private readonly IMinioClient _minioClient;
        private readonly ILogger<MinioProvider> _logger;

        public MinioProvider(
            IMinioClient minioClient, ILogger<MinioProvider> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }

        public async Task<Result<string, Error>> UploadFile(
            FileData fileData,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketExistsArgs = new BucketExistsArgs()
                   .WithBucket(fileData.BucketName);

                var bucketExists = await _minioClient
                    .BucketExistsAsync(bucketExistsArgs, cancellationToken);

                if (!bucketExists)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(fileData.BucketName);

                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }

                var path = Guid.NewGuid();

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithStreamData(fileData.Stream)
                    .WithObjectSize(fileData.Stream.Length)
                    .WithObject(path.ToString());

                var result = await _minioClient
                    .PutObjectAsync(putObjectArgs, cancellationToken);

                return result.ObjectName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to upload file in minio");
                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
        }

        public async Task<Result<string, Error>> DeleteFile(
            FileMeta fileMeta,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(fileMeta.BucketName)
                    .WithObject(fileMeta.ObjectName);

                await _minioClient
                    .RemoveObjectAsync(removeObjectArgs, cancellationToken);

                return fileMeta.ObjectName;
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
                    .WithObject(fileMeta.ObjectName)
                    .WithExpiry(PERSIGNED_EXPIRATION_TIME);

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
