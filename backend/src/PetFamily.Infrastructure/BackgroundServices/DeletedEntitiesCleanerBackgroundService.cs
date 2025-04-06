using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Infrastructure.Options;

namespace PetFamily.Infrastructure.BackgroundServices
{
    public class DeletedEntitiesCleanerBackgroundService : BackgroundService
    {
        private readonly ILogger<DeletedEntitiesCleanerBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _timeSpan = TimeSpan.FromHours(24);
        private readonly int _daysBeforeDeletion;

        public DeletedEntitiesCleanerBackgroundService(
            ILogger<DeletedEntitiesCleanerBackgroundService> logger,
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

                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var expiredVolunteers = await dbContext
                    .Volunteers
                    .Where(v =>
                        v.IsDeleted &&
                        v.DeletionDate != null
                        && v.DeletionDate.Value.AddDays(_daysBeforeDeletion) < DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                var expiredSpecies = await dbContext
                    .Species
                    .Where(s =>
                        s.IsDeleted &&
                        s.DeletionDate != null
                        && s.DeletionDate.Value.AddDays(_daysBeforeDeletion) < DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                dbContext.Volunteers.RemoveRange(expiredVolunteers);
                dbContext.Species.RemoveRange(expiredSpecies);

                await dbContext.SaveChangesAsync();

                _logger.LogInformation("Deleted entities cleaner background service has worked");
                await Task.Delay(_timeSpan, stoppingToken);
            }

            await Task.CompletedTask;
        }
    }
}
