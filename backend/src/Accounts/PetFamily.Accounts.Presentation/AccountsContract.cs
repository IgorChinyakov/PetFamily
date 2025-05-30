using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Contracts;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Presentation
{
    public class AccountsContract : IAccountsContract
    {
        private readonly IPermissionManager _permissionManager;

        public AccountsContract(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        public async Task<UnitResult<ErrorsList>> CheckPermissionByUserId(
            Guid userId, string permisssionCode, CancellationToken cancellationToken = default)
            => await _permissionManager.CheckPermissionByUserId(userId, permisssionCode, cancellationToken);
    }
}
