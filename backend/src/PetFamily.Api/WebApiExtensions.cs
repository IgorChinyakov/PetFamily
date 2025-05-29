using PetFamily.Accounts.Infrastructure.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Web
{
    public static class WebApiExtensions
    {
        public static async Task<WebApplication> ExecuteSeeder(this WebApplication webApplication)
        {
            var seeder = webApplication.Services
                .GetRequiredService<AccountsSeeder>();

            await seeder.SeedAsync();

            return webApplication;
        }
    }
}
