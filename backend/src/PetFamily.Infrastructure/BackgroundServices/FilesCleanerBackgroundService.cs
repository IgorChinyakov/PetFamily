using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Infrastructure.BackgroundServices
{
    public class FilesCleanerBackgroundService : BackgroundService
    {
        private readonly ILogger<FilesCleanerBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMessageQueue<IEnumerable<FileMeta>> _messageQueue;

        public FilesCleanerBackgroundService(
            ILogger<FilesCleanerBackgroundService> logger,
            IServiceScopeFactory scopeFactory,
            IMessageQueue<IEnumerable<FileMeta>> messageQueue)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _messageQueue = messageQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Minio files cleaner service has started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await using var scope = _scopeFactory.CreateAsyncScope();

                var filesProvider = scope.ServiceProvider.GetRequiredService<IFilesProvider>();

                var paths = await _messageQueue.ReadAsync(stoppingToken);

                foreach (var path in paths)
                    await filesProvider.RemoveFile(path, stoppingToken);

                _logger.LogInformation("Minio files cleaner service has removed unsuccesfully uploaded files");
            }
        }
    }
}
