using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Infrastructure.Authorization
{
    public class PermissionManager : IPermissionManager
    {
        private readonly AccountDbContext _accountDbContext;

        public PermissionManager(AccountDbContext accountDbContext)
        {
            _accountDbContext = accountDbContext;
        }

        public async Task AddRangeIfExists(IEnumerable<string> permissionsToAdd)
        {
            foreach (var permissionCode in permissionsToAdd)
            {
                var isPermissionExist = await _accountDbContext.Permissions
                    .AnyAsync(p => p.Code == permissionCode);
                if (isPermissionExist)
                    continue;

                await _accountDbContext.Permissions.AddAsync(new Permission { Code = permissionCode });
            }

            await _accountDbContext.SaveChangesAsync();
        }

        public async Task<Permission?> FindByCode(string code)
        {
            return await _accountDbContext.Permissions.FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<UnitResult<ErrorsList>> CheckPermissionByUserId(
            Guid userId,
            string permissionCode,
            CancellationToken cancellationToken = default)
        {
            var user = await _accountDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return Errors.General.NotFound(userId).ToErrorsList();

            var userPermisssions = await _accountDbContext.Roles
                .Join(_accountDbContext.Users, r => r.Id, u => u.RoleId, (r, u) => new
                {
                    UserId = u.Id,
                    Permissions = r.Permissions
                }).FirstOrDefaultAsync(up => up.UserId == userId, cancellationToken);

            if (userPermisssions == null)
                return Errors.User.AccessDenied(userId).ToErrorsList();

            if(userPermisssions.Permissions.Select(p => p.Code).Contains(permissionCode))
                return Errors.User.AccessDenied(userId).ToErrorsList();

            return Result.Success<ErrorsList>();
        }
    }
}
