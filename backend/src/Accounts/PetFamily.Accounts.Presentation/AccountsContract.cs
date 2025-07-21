using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Application.Accounts.GetAccountsData;
using PetFamily.Accounts.Application.AccountsFeatures.CreateVolunteerAccount;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Presentation
{
    public class AccountsContract : IAccountsContract
    {
        private readonly IPermissionManager _permissionManager;
        private readonly GetUserAccountsDataHandler _getUserAccountsDataHandler;
        private readonly CreateVolunteerAccountHandler _createVolunteerAccountHandler;


        public AccountsContract(
            IPermissionManager permissionManager,
            GetUserAccountsDataHandler getUserAccountsDataHandler,
            CreateVolunteerAccountHandler createVolunteerAccountHandler)
        {
            _permissionManager = permissionManager;
            _getUserAccountsDataHandler = getUserAccountsDataHandler;
            _createVolunteerAccountHandler = createVolunteerAccountHandler;
        }

        public async Task<UnitResult<ErrorsList>> CheckPermissionByUserId(
            Guid userId, string permisssionCode, CancellationToken cancellationToken = default)
            => await _permissionManager.CheckPermissionByUserId(userId, permisssionCode, cancellationToken);

        public async Task<Result<AccountsDataResponse, ErrorsList>> GetUserAccountsData(
            Guid userId, CancellationToken token = default)
            => await _getUserAccountsDataHandler.Handle(new GetUserAccountsDataCommand(userId), token);

        public async Task<Result<Guid, ErrorsList>> CreateVolunteerAccount(
            CreateVolunteerAccountRequest request,
            CancellationToken token = default)
            => await _createVolunteerAccountHandler.Handle(
                new CreateVolunteerAccountCommand(request.UserId), token);
    }
}
