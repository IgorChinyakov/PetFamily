using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public DbSet<AdminAccount> AdminAccounts => Set<AdminAccount>();

        public DbSet<ParticipantAccount> ParticipantAccounts => Set<ParticipantAccount>();

        public DbSet<VolunteerAccount> VolunteerAccounts => Set<VolunteerAccount>();

        public DbSet<Permission> Permissions => Set<Permission>();

        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

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
                .Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany()
                .UsingEntity<IdentityUserRole<Guid>>();

            modelBuilder.Entity<User>()
                .Property(u => u.SocialMedia)
                .HasConversion(
                    socialMedia => JsonSerializer.Serialize(socialMedia, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<List<SocialMedia>>(json, JsonSerializerOptions.Default)!,
                   new ValueComparer<List<SocialMedia>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                        c => c.ToList()))
                .HasColumnName("social_media")
                .HasColumnType("Jsonb");

            modelBuilder.Entity<VolunteerAccount>()
                .Property(u => u.Details)
                .HasConversion(
                    details => JsonSerializer.Serialize(details, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<List<Details>>(json, JsonSerializerOptions.Default)!,
                   new ValueComparer<List<Details>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                        c => c.ToList()))
                .HasColumnName("details")
                .HasColumnType("Jsonb");

            modelBuilder.Entity<User>()
                .ComplexProperty(u => u.FullName, builder =>
                {
                    builder.Property(fn => fn.Name).HasColumnName("name");
                    builder.Property(fn => fn.SecondName).HasColumnName("second_name");
                    builder.Property(fn => fn.FamilyName).HasColumnName("family_name");
                });

            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.Role)
            //    .WithMany()
            //    .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<VolunteerAccount>()
                .HasOne(u => u.User)
                .WithOne(vc => vc.VolunteerAccount)
                .HasForeignKey<VolunteerAccount>(u => u.UserId);

            modelBuilder.Entity<AdminAccount>()
                .HasOne(u => u.User)
                .WithOne(vc => vc.AdminAccount)
                .HasForeignKey<AdminAccount>(u => u.UserId);

            modelBuilder.Entity<ParticipantAccount>()
                .HasOne(u => u.User)
                .WithOne(vc => vc.ParticipantAccount)
                .HasForeignKey<ParticipantAccount>(u => u.UserId);

            modelBuilder.Entity<ParticipantAccount>()
                .Property(u => u.FavoritePets)
                .HasConversion(
                    favoritePets => JsonSerializer.Serialize(favoritePets, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<List<Guid>>(json, JsonSerializerOptions.Default)!,
                   new ValueComparer<List<Guid>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                        c => c.ToList()))
                .HasColumnName("favorite_pets")
                .HasColumnType("Jsonb");

            modelBuilder
                .Entity<Role>().ToTable("roles");

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<RolePermission>(rp => rp.ToTable("role_permissions"));

            modelBuilder
                .Entity<Permission>().ToTable("permissions");

            modelBuilder.Entity<Permission>()
                .HasIndex(p => p.Code).IsUnique();

            modelBuilder
                .Entity<Permission>().HasKey(p => p.Id);

            modelBuilder
                .Entity<AdminAccount>().ToTable("admin_accounts");

            modelBuilder
                .Entity<AdminAccount>().HasKey(p => p.Id);

            modelBuilder
               .Entity<ParticipantAccount>().ToTable("participant_accounts");

            modelBuilder
                .Entity<ParticipantAccount>().HasKey(p => p.Id);

            modelBuilder
               .Entity<VolunteerAccount>().ToTable("volunteer_accounts");

            modelBuilder
               .Entity<VolunteerAccount>().HasKey(p => p.Id);

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

            modelBuilder.HasDefaultSchema("accounts");
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => builder.AddConsole());
    }
}
