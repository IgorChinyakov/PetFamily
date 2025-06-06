using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Infrastructure.Providers
{
    public class JwtProvider : ITokenProvider
    {
        private readonly JwtSettings _jwtSettings;
        private readonly AccountDbContext _accountDbContext;

        public JwtProvider(
            IOptions<JwtSettings> jwtSettings, 
            AccountDbContext accountDbContext)
        {
            _jwtSettings = jwtSettings.Value;
            _accountDbContext = accountDbContext;
        }

        public JwtTokenResult GenerateAccessToken(
            User user, 
            CancellationToken cancellationToken = default)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var jti = Guid.NewGuid();

            Claim[] claims = [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jti.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            ]; 

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenLifeTime),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                claims: claims);

            var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new JwtTokenResult(stringToken, jti);
        }

        public async Task<Guid> GenerateRefreshToken(
            User user, 
            Guid jti,
            CancellationToken cancellationToken = default)
        {
            var token = Guid.NewGuid();

            var refreshSession = new RefreshSession
            {
                User = user,
                Jti = jti,
                CreatedAt = DateTime.UtcNow,
                ExpiresIn = DateTime.UtcNow.AddDays(30),
                RefreshToken = token
            };

            _accountDbContext.RefreshSessions.Add(refreshSession);
            await _accountDbContext.SaveChangesAsync(cancellationToken);

            return refreshSession.RefreshToken;
        }

        public async Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaims(
            string accessToken,
            CancellationToken cancellationToken = default)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters()
            {
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
            };

            var validationResult = await jwtHandler.ValidateTokenAsync(accessToken, validationParameters);
            if (!validationResult.IsValid)
                return Errors.Authorization.InvalidToken();

            return validationResult.ClaimsIdentity.Claims.ToList();
        }
    }
}
