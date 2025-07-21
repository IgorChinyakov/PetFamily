using Microsoft.Extensions.DependencyInjection;
using PetFamily.Discussions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDiscussionsContracts(
            this IServiceCollection services)
        {
            services.AddScoped<IDiscussionsContract, DiscussionsContract>();

            return services;
        }
    }
}
