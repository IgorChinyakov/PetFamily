using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Options;
using PetFamily.Specieses.Infrastructure.DbContexts;

namespace PetFamily.Specieses.Infrastructure.BackgroundServices
{
    public class DeletedSpeciesCleanerBackgroundService : BackgroundService
    {
        private readonly ILogger<DeletedSpeciesCleanerBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _timeSpan = TimeSpan.FromHours(24);
        private readonly int _daysBeforeDeletion;

        public DeletedSpeciesCleanerBackgroundService(
            ILogger<DeletedSpeciesCleanerBackgroundService> logger,
            IServiceScopeFactory scopeFactory,
            IOptions<CleanUpSettings> options)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _daysBeforeDeletion = options.Value.DaysBeforeDeletion;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Deleted entities cleaner background service has started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await using var scope = _scopeFactory.CreateAsyncScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
                var unitOfWork = scope.ServiceProvider.GetRequiredKeyedService<IUnitOfWork>(UnitOfWorkKeys.Species);

                var species = dbContext
                    .Species
                    .Include(s => s.Breeds);

                var expiredSpecies = await species
                    .Where(s =>
                        s.IsDeleted &&
                        s.DeletionDate != null
                        && s.DeletionDate.Value.AddDays(_daysBeforeDeletion) < DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                dbContext.Species.RemoveRange(expiredSpecies);

                await unitOfWork.SaveChanges();

                _logger.LogInformation("Deleted entities cleaner background service has worked");
                await Task.Delay(_timeSpan, stoppingToken);
            }

            await Task.CompletedTask;
        }
    }
}
