using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Options;
using PetFamily.Volunteers.Infrastructure;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.BackgroundServices
{
    public class DeletedVolunteersCleanerBackgroundService : BackgroundService
    {
        private readonly ILogger<DeletedVolunteersCleanerBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _timeSpan = TimeSpan.FromHours(24);
        private readonly int _daysBeforeDeletion;

        public DeletedVolunteersCleanerBackgroundService(
            ILogger<DeletedVolunteersCleanerBackgroundService> logger,
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

                var dbContext = scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
                var unitOfWork = scope.ServiceProvider.GetRequiredKeyedService<IUnitOfWork>(UnitOfWorkKeys.Volunteers);

                var volunteers = dbContext
                    .Volunteers
                    .Include(v => v.Pets);

                var expiredVolunteers = await volunteers
                    .Where(v =>
                        v.IsDeleted &&
                        v.DeletionDate != null
                        && v.DeletionDate.Value.AddDays(_daysBeforeDeletion) < DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                dbContext.Volunteers.RemoveRange(expiredVolunteers);

                await unitOfWork.SaveChanges();

                _logger.LogInformation("Deleted entities cleaner background service has worked");
                await Task.Delay(_timeSpan, stoppingToken);
            }

            await Task.CompletedTask;
        }
    }
}
