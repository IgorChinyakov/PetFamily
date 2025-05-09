using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.DTOs;
using PetFamily.Specieses.Contracts.DTOs;

namespace PetFamily.Volunteers.Infrastructure.DbContexts
{

    public class SpeciesReadDbContext : DbContext, ISpeciesReadDbContext
    {
        private readonly string _connectionString;

        public SpeciesReadDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();
        public IQueryable<BreedDto> Breeds => Set<BreedDto>();

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
                typeof(SpeciesReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => builder.AddConsole());
    }
}
