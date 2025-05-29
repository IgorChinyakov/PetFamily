using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Infrastructure.DbContexts
{

    public class VolunteersReadDbContext : DbContext, IVolunteersReadDbContext
    {
        private readonly string _connectionString;

        public VolunteersReadDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
        public IQueryable<PetDto> Pets => Set<PetDto>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(VolunteersReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);

            modelBuilder.HasDefaultSchema("pet_management");
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => builder.AddConsole());
    }
}
