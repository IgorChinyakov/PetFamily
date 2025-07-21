using CSharpFunctionalExtensions;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Contracts
{
    public interface IAccountsContract
    {
        Task<UnitResult<ErrorsList>> CheckPermissionByUserId(Guid userId, string permisssionCode, CancellationToken cancellationToken = default);

        Task<Result<AccountsDataResponse, ErrorsList>> GetUserAccountsData(Guid userId, CancellationToken token = default);

        Task<Result<Guid, ErrorsList>> CreateVolunteerAccount(CreateVolunteerAccountRequest request, CancellationToken token = default);
    }
}