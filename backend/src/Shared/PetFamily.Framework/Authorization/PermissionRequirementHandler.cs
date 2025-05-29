using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using PetFamily.Accounts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Framework.Authorization
{
    public class PermissionRequirementHandler : 
        AuthorizationHandler<PermissionAttribute>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PermissionAttribute permission)
        {
            var subClaim = context.User.Claims
                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if(subClaim == null || Guid.TryParse(subClaim.Value, out Guid userId))
                return;

            using var scope = _serviceScopeFactory.CreateScope();
            var accountsContract = scope.ServiceProvider.GetRequiredService<IAccountsContract>();

            var result = await accountsContract.CheckPermissionByUserId(userId, permission.Code);
            if (result.IsFailure)
                return;

            context.Succeed(permission);
        }
    }
}
