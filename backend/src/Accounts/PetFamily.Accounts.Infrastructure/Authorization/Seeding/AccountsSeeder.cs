using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Accounts.Infrastructure.Authorization.Managers;
using PetFamily.Accounts.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Infrastructure.Authorization.Seeding
{
    public class AccountsSeeder
    {
        private const string ACCOUNTS_PATHS = "etc/accounts.json";
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AccountsSeeder> _logger;

        public AccountsSeeder(
            IServiceScopeFactory serviceScopeFactory, 
            ILogger<AccountsSeeder> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            _logger.LogInformation("Seeding accounts");

            using var scope = _serviceScopeFactory.CreateScope();

            var json = await File.ReadAllTextAsync(ACCOUNTS_PATHS);

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var adminOptions = scope.ServiceProvider.GetRequiredService<IOptions<AdminOptions>>().Value;
            var adminAccountManager = scope.ServiceProvider.GetRequiredService<AdminAccountManager>();
            var accountsContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var permissionManager = scope.ServiceProvider.GetRequiredService<IPermissionManager>();
            var rolePermissionManager = scope.ServiceProvider.GetRequiredService<RolePermissionManager>();

            var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json)
                ?? throw new ApplicationException("couldn't deserialize role permission config");

            await SeedPermissions(permissionManager, seedData);

            await SeedRoles(roleManager, seedData);

            await SeedRolePermissions(roleManager, rolePermissionManager, seedData);

            await SeedAdmin(userManager, adminOptions, adminAccountManager, roleManager, accountsContext);
        }

        private async Task SeedAdmin(
            UserManager<User> userManager, 
            AdminOptions adminOptions, 
            AdminAccountManager adminAccountManager, 
            RoleManager<Role> roleManager,
            AccountDbContext accountDbContext)
        {
            var transaction = await accountDbContext.Database.BeginTransactionAsync();

            var adminRole = await roleManager.FindByNameAsync(AdminAccount.ADMIN)
                            ?? throw new ApplicationException("Could not find admin role.");

            var adminExists = await userManager.FindByEmailAsync(adminOptions.Email);
            if (adminExists != null)
            {
                await transaction.RollbackAsync();
                return;
            }

            var fullName = new FullName
            {
                Name = adminOptions.FirstName,
                SecondName = adminOptions.SecondName,
                FamilyName = adminOptions.LastName
            };
            var adminUserResult = User.CreateAdmin(adminOptions.UserName, adminOptions.Email, fullName, adminRole);
            if (adminUserResult.IsFailure)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("trying to create user with invalid role. Role must be admin");
            }

            await userManager.CreateAsync(adminUserResult.Value, adminOptions.Password);

            var adminAccount = new AdminAccount(adminUserResult.Value);
            await adminAccountManager.CreateAdminAccount(adminAccount);

            await transaction.CommitAsync();
        }

        private async Task SeedRolePermissions( 
            RoleManager<Role> roleManager, 
            RolePermissionManager rolePermissionManager, 
            RolePermissionConfig seedData)
        {
            foreach (var roleName in seedData.Roles.Keys)
            {
                var role = await roleManager.FindByNameAsync(roleName);

                var rolePermissions = seedData.Roles[roleName];

                await rolePermissionManager.AddRangeIfExists(role!.Id, rolePermissions);

                _logger.LogInformation("role permissions added to database");
            }
        }

        private async Task SeedPermissions(IPermissionManager permissionManager, RolePermissionConfig seedData)
        {
            var permissionsToAdd = seedData.Permissions.SelectMany(permissionGroup => permissionGroup.Value);

            await permissionManager.AddRangeIfExists(permissionsToAdd);

            _logger.LogInformation("permissions added to database");
        }

        private async Task SeedRoles(RoleManager<Role> roleManager, RolePermissionConfig seedData)
        {
            foreach (var role in seedData.Roles.Keys)
            {
                var isRoleExists = await roleManager.RoleExistsAsync(role);
                if (isRoleExists)
                    continue;

                await roleManager.CreateAsync(new Role { Name = role});
            }

            _logger.LogInformation("roles added to database");
        }
    }
}
