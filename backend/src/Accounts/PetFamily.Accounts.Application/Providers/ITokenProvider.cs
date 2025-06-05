using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Providers
{
    public interface ITokenProvider
    {
        JwtTokenResult GenerateAccessToken(User user, CancellationToken cancellationToken = default);

        Task<Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken = default);

        Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaims(string accessToken, CancellationToken cancellationToken = default);
    }
}
