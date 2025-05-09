using Microsoft.Extensions.DependencyInjection;
using PetFamily.Files.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Files.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFilesContracts(
            this IServiceCollection services)
        {
            services.AddScoped<IFilesContract, FilesContract>();

            return services;
        }
    }
}
