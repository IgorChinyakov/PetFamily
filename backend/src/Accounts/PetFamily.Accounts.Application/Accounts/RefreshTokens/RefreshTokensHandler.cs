using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Accounts.RefreshTokens
{
    public class RefreshTokensHandler :
        ICommandHandler<LoginResponse, RefreshTokensCommand>
    {
        private readonly IRefreshSessionManager _refreshSessionManager;
        private readonly ITokenProvider _tokenProvider;

        public RefreshTokensHandler(
            IRefreshSessionManager refreshSessionManager, 
            ITokenProvider tokenProvider)
        {
            _refreshSessionManager = refreshSessionManager;
            _tokenProvider = tokenProvider;
        }

        public async Task<Result<LoginResponse, ErrorsList>> Handle(
            RefreshTokensCommand command,
            CancellationToken cancellationToken = default)
        {
            var oldRefreshSession = await _refreshSessionManager
                .GetByRefreshToken(command.RefreshToken, cancellationToken);

            if(oldRefreshSession.IsFailure)
                return oldRefreshSession.Error.ToErrorsList();

            if (oldRefreshSession.Value.ExpiresIn < DateTime.UtcNow)
                return Errors.Authorization.ExpiredToken().ToErrorsList();

            var userClaims = await _tokenProvider.GetUserClaims(command.AccessToken, cancellationToken);
            if(userClaims.IsFailure)
                return userClaims.Error.ToErrorsList();

            var userIdString = userClaims.Value.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if(!Guid.TryParse(userIdString, out var userId))
                return Errors.General.NotFound().ToErrorsList();

            if (oldRefreshSession.Value.UserId != userId)
                return Errors.Authorization.InvalidToken().ToErrorsList();

            var userJtiString = userClaims.Value.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (!Guid.TryParse(userJtiString, out var userJtiGuid))
                return Errors.General.NotFound().ToErrorsList();

            if (oldRefreshSession.Value.Jti != userJtiGuid)
                return Errors.Authorization.InvalidToken().ToErrorsList();

            await _refreshSessionManager.Delete(oldRefreshSession.Value);

            var accessToken = _tokenProvider
                .GenerateAccessToken(oldRefreshSession.Value.User, cancellationToken);
            var refreshToken = await _tokenProvider
                .GenerateRefreshToken(oldRefreshSession.Value.User, accessToken.Jti, cancellationToken);

            return new LoginResponse(accessToken.AccessToken, refreshToken);
        }
    }
}
