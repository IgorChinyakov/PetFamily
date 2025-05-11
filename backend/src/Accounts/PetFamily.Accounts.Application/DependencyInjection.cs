using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using System.Text;

namespace PetFamily.Accounts.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountsApplication(
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

            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
