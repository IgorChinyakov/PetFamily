using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Application.Specieses;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure
{
    public static class Inject
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<WriteDbContext>();
            services.AddScoped<IReadDbContext, ReadDbContext>();

            services.AddScoped<IVolunteerRepository, VolunteerRepository>();

            services.AddScoped<ISpeciesRepository, SpeciesRepository>();

            services.AddScoped<IFilesProvider, MinioProvider>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddHostedService<DeletedEntitiesCleanerBackgroundService>();
            services.AddHostedService<FilesCleanerBackgroundService>();

            services.AddSingleton<IMessageQueue<IEnumerable<FileMeta>>, InMemoryMessageQueue<IEnumerable<FileMeta>>>();

            services.AddMinio(configuration);

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
