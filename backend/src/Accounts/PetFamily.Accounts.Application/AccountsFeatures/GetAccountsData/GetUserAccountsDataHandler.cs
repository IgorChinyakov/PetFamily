using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Contracts.DTOs;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Accounts.GetAccountsData
{
    public class GetUserAccountsDataHandler : 
        ICommandHandler<AccountsDataResponse, GetUserAccountsDataCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAdminAccountManager _adminAccountManager;

        public GetUserAccountsDataHandler(
            UserManager<User> userManager, 
            IAdminAccountManager adminAccountManager)
        {
            _userManager = userManager;
            _adminAccountManager = adminAccountManager;
        }

        public async Task<Result<AccountsDataResponse, ErrorsList>> Handle(
            GetUserAccountsDataCommand command, 
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                .Include(u => u.VolunteerAccount)
                .Include(u => u.ParticipantAccount)
                .Include(u => u.AdminAccount)
                .FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);
            if (user == null)
                return Errors.General.NotFound(command.Id).ToErrorsList();

            AdminAccountDto? adminAccount = null;
            if (user.AdminAccount != null)
                adminAccount = new AdminAccountDto(
                    user.AdminAccount.Id, user.AdminAccount.UserId);

            ParticipantAccountDto? participantAccount = null;
            if (user.ParticipantAccount != null)
                participantAccount = new ParticipantAccountDto(
                    user.ParticipantAccount.Id, user.ParticipantAccount.UserId, user.ParticipantAccount.FavoritePets);

            VolunteerAccountDto? volunteerAccount = null;
            if (user.VolunteerAccount != null)
                volunteerAccount = new VolunteerAccountDto(
                    user.VolunteerAccount.Id, 
                    user.VolunteerAccount.UserId, 
                    user.VolunteerAccount.Experience, 
                    user.VolunteerAccount.Details
                    .Select(d => new DetailsDto(d.Title, d.Description)).ToList());

            var resposne = new AccountsDataResponse(participantAccount, volunteerAccount, adminAccount);

            return resposne;
        }
    }
}
