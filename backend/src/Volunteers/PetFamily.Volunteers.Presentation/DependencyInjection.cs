using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddVolunteersContracts(
            this IServiceCollection services)
        {
            services.AddScoped<IVolunteersContract, VolunteersContarct>();

            return services;
        }
    }
}
