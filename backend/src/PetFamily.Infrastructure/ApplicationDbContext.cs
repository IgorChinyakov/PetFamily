using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Infrastructure
{
    public class ApplicationDbContext(IConfiguration configuration) : DbContext
    {
        private const string DATABASE = nameof(Database);

        public DbSet<Volunteer> Volunteers => Set<Volunteer>();
        public DbSet<Species> Species => Set<Species>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => builder.AddConsole());
    }
}
