using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Specieses.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSpeciesApplication(
            this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes
                    .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes
                    .AssignableToAny(typeof(IQueryHandler<,>), typeof(IQueryHandlerWithResult<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}
