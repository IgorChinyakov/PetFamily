using Microsoft.Extensions.DependencyInjection;
using PetFamily.Discussions.Contracts;
using PetFamily.VolunteerRequests.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddVolunteerRequestsContracts(
            this IServiceCollection services)
        {
            services.AddScoped<IVolunteerRequestsContract, VolunteerRequestsContract>();

            return services;
        }
    }
}
