using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Core.Options;
using PetFamily.Files.Application;
using PetFamily.Files.Application.Messaging;
using PetFamily.Files.Contracts.DTOs;
using PetFamily.Files.Infrastructure.BackgroundServices;
using PetFamily.Files.Infrastructure.MessageQueues;
using PetFamily.Files.Infrastructure.Providers;

namespace PetFamily.Files.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFilesInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddFilesProvider()
                .AddBackgroundServices()
                .AddMinio(configuration);

            services
                .AddSingleton<IMessageQueue<IEnumerable<FileMeta>>, InMemoryMessageQueue<IEnumerable<FileMeta>>>();

            return services;
        }

        private static IServiceCollection AddFilesProvider(this IServiceCollection services)
        {
            services.AddScoped<IFilesProvider, MinioProvider>();

            return services;
        }

        private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<FilesCleanerBackgroundService>();

            return services;
        }

        private static IServiceCollection AddMinio(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MinioOptions>
                (configuration.GetSection(MinioOptions.MINIO));

            services.AddMinio(options =>
            {
                var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                ?? throw new ApplicationException("Missing minio configuration");

                options.WithEndpoint(minioOptions.Endpoint);
                options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
                options.WithSSL(minioOptions.WithSsl);
            });

            return services;
        }
    }
}
