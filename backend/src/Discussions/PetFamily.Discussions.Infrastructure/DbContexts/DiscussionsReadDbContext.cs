using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Contracts.DTOs;
using PetFamily.Discussions.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Infrastructure.DbContexts
{
    public class DiscussionsReadDbContext : DbContext, IDiscussionsReadDbContext
    {
        private readonly string _connectionString;

        public DiscussionsReadDbContext(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }

        public IQueryable<DiscussionDto> Discussions => Set<DiscussionDto>();

        public IQueryable<MessageDto> Messages => Set<MessageDto>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(DiscussionsWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);

            modelBuilder.HasDefaultSchema("discussions");
        }

        private ILoggerFactory CreateLoggerFactory()
            => LoggerFactory.Create(builder => builder.AddConsole());
    }
}
