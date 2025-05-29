using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Infrastructure.Authorization
{
    public class RolePermissionManager
    {
        private readonly AccountDbContext _accountDbContext;

        public RolePermissionManager(AccountDbContext accountDbContext)
        {
            _accountDbContext = accountDbContext;
        }

        public async Task AddRangeIfExists(Guid roleId, IEnumerable<string> permissions)
        {
            foreach (var permissionCode in permissions)
            {
                var permission = await _accountDbContext.Permissions.FirstOrDefaultAsync(p => p.Code == permissionCode);

                var rolePermissionExists = await _accountDbContext.RolePermissions
                    .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission!.Id);

                if (rolePermissionExists)
                    continue;

                await _accountDbContext.RolePermissions
                    .AddAsync(new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permission!.Id
                    });
            }

            await _accountDbContext.SaveChangesAsync();
        }
    }
}
