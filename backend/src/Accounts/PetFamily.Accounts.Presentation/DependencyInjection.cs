﻿using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountsContracts(
            this IServiceCollection services)
        {
            services.AddScoped<IAccountsContract, AccountsContract>();

            return services;
        }
    }
}
