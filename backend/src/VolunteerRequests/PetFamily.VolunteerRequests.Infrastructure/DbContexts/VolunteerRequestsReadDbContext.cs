using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.DbContexts
{
    public class VolunteerRequestsReadDbContext : DbContext, IVolunteerRequestsReadDbContext
    {
        private readonly string _connectionString;

        public VolunteerRequestsReadDbContext(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }

        public IQueryable<VolunteerRequestDto> RequestDtos => Set<VolunteerRequestDto>();

        public IQueryable<RejectedRequestDto> RejectedRequestsDtos => Set<RejectedRequestDto>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(VolunteerRequestsReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);

            modelBuilder.HasDefaultSchema("volunteer_requests");
        }

        private ILoggerFactory CreateLoggerFactory()
            => LoggerFactory.Create(builder => builder.AddConsole());
    }
}
