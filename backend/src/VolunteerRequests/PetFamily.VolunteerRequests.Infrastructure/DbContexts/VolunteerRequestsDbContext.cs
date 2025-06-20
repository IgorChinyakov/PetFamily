using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.VolunteerRequests.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.DbContexts
{
    public class VolunteerRequestsDbContext : DbContext
    {
        private readonly string _connectionString;

        public VolunteerRequestsDbContext(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }

        public DbSet<VolunteerRequest> VolunteerRequests => Set<VolunteerRequest>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(VolunteerRequestsDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);

            modelBuilder.HasDefaultSchema("volunteer_requests");
        }

        private ILoggerFactory CreateLoggerFactory()
            => LoggerFactory.Create(builder => builder.AddConsole());
    }
}
