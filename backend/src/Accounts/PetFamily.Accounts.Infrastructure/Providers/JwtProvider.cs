using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Authorization;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Options;
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

        public JwtProvider(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateAccessToken(
            User user, CancellationToken cancellationToken = default)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            Claim[] claims = [
                new Claim(CustomClaims.Sub, user.Id.ToString()),
                new Claim(CustomClaims.Email, user.Email!)
            ]; 

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenLifeTime),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
                claims: claims);

            var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return stringToken;
        }
    }
}
