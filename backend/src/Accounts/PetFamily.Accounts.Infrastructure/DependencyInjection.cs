﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Infrastructure.Authorization;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.Providers;
using PetFamily.SharedKernel;
using System.Text;
using static CSharpFunctionalExtensions.Result;

namespace PetFamily.Accounts.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountsInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenProvider, JwtProvider>();

            services
                .AddIdentity();

            services.AddScoped
                (_ => new AccountDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

            services
                .Configure<JwtSettings>(configuration.GetSection(JwtSettings.JwtPath));

            services
                .AddAuthenticationAndBearer(configuration);

            services.AddSingleton<AccountsSeeder>();

            services
                .AddAuthorizationAndPolicies();

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

            services.AddScoped<IPermissionManager, PermissionManager>();
            services.AddScoped<RolePermissionManager>();

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
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        private static IServiceCollection AddAuthorizationAndPolicies(
            this IServiceCollection services)
        {
            services.AddAuthorization(options => 
            { 
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireClaim("role", "user")
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services;
        }
    }
}
