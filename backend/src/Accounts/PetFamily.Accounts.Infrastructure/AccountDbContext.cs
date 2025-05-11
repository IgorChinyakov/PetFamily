using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Infrastructure
{
    public class AccountDbContext : 
        IdentityDbContext<User, Role, Guid>
    {
        private readonly string _connectionString;

        public AccountDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<User>().ToTable("users");

            modelBuilder
                .Entity<Role>().ToTable("roles");

            modelBuilder
                .Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");

            modelBuilder
                .Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");

            modelBuilder
                .Entity<IdentityUserRole<Guid>>().ToTable("user_roles");

            modelBuilder
                .Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");

            modelBuilder
                .Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => builder.AddConsole());
    }
}
