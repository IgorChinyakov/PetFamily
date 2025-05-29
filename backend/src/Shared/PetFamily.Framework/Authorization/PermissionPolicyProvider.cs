using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Framework.Authorization
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (string.IsNullOrWhiteSpace(policyName))
                return null;

            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new PermissionAttribute(policyName))
                .Build();
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => Task.FromResult<AuthorizationPolicy?>(null);
    }
}
