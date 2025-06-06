using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Providers
{
    public interface IRefreshSessionManager
    {
        Task<Result<RefreshSession, Error>> GetByRefreshToken(
            Guid refreshToken, CancellationToken cancellationToken = default);

        Task Delete(RefreshSession refreshSession, CancellationToken cancellationToken = default);
    }
}