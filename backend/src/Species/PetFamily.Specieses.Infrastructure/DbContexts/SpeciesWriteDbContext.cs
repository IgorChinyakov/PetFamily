using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.DTOs;
using PetFamily.Specieses.Domain.Entities;

namespace PetFamily.Volunteers.Infrastructure.DbContexts
{
    public class SpeciesWriteDbContext : DbContext
    {
        private readonly string _connectionString;

        public SpeciesWriteDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Species> Species => Set<Species>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SpeciesWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => builder.AddConsole());
    }
}
