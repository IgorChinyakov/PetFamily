using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Contracts
{
    public interface IAccountsContract
    {
        Task<UnitResult<ErrorsList>> CheckPermissionByUserId(Guid userId, string permisssionCode, CancellationToken cancellationToken = default);
    }
}