using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Models;
using PetFamily.Framework.Authorization;
using PetFamily.Framework.Filters;
using PetFamily.Framework.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Framework
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFramework(
            this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddHttpContextAccessor();
            services.AddScoped<UserScopedDataProccessor>();
            services.AddScoped<UserScopedDataFilter>();

            return services;
        }
    }
}
