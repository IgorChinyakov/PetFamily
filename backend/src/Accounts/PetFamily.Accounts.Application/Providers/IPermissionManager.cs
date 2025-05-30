using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Providers
{
    public interface IPermissionManager
    {
        Task<Permission?> FindByCode(string code);

        Task<UnitResult<ErrorsList>> CheckPermissionByUserId(
            Guid userId,
            string permissionCode,
            CancellationToken cancellationToken = default);

        Task AddRangeIfExists(IEnumerable<string> permissionsToAdd);
    }
}