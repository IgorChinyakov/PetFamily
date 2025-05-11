using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Authorization;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.Providers;
using PetFamily.SharedKernel;
using System.Text;
using static CSharpFunctionalExtensions.Result;

namespace PetFamily.Volunteers.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountsInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenProvider, JwtProvider>();

            services
                .AddIdentity();

            services.AddScoped<AccountDbContext>
                (_ => new AccountDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            services
                .Configure<JwtSettings>(configuration.GetSection(JwtSettings.JwtPath));

            services
                .AddAuthenticationAndBearer(configuration);

            return services;
        }

        private static IServiceCollection AddIdentity(
            this IServiceCollection services)
        {
            services
                .AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<AccountDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        private static IServiceCollection AddAuthenticationAndBearer(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(options =>
            {
                    var jwtSettings = configuration.GetSection(JwtSettings.JwtPath).Get<JwtSettings>()
                    ?? throw new ApplicationException("Missing configuration");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                        ValidateLifetime = true
                    };
                });

            return services;
        }
    }
}
