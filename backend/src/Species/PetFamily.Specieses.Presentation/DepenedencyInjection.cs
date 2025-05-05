using Microsoft.Extensions.DependencyInjection;
using PetFamily.Specieses.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Specieses.Presentation
{
    public static class DepenedencyInjection
    {
        public static IServiceCollection AddSpeciesContracts(
            this IServiceCollection services)
        {
            services.AddScoped<ISpeciesContract, SpeciesContract>();

            return services;
        }
    }
}
