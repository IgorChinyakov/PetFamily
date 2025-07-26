using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.AccountsFeatures.CreateVolunteerAccount
{
    public class CreateVolunteerAccountHandler : 
        ICommandHandler<Guid, CreateVolunteerAccountCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteerAccountManager _volunteerAccountManager;
        private readonly ILogger<CreateVolunteerAccountHandler> _logger;
        private readonly IValidator<CreateVolunteerAccountCommand> _validator;

        public CreateVolunteerAccountHandler(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<CreateVolunteerAccountHandler> logger,
            IVolunteerAccountManager volunteerAccountManager,
            [FromKeyedServices(UnitOfWorkKeys.Accounts)] IUnitOfWork unitOfWork,
            IValidator<CreateVolunteerAccountCommand> validator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _volunteerAccountManager = volunteerAccountManager;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            CreateVolunteerAccountCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var user = await _userManager.FindByIdAsync(command.UserId.ToString());
            if (user == null)
                return Errors.General.NotFound(command.UserId).ToErrorsList();

            var transaction = await _unitOfWork.BeginTransaction();

            var result = await _userManager.AddToRoleAsync(user, VolunteerAccount.VOLUNTEER);
            if (!result.Succeeded)
            {
                transaction.Rollback();
                return Error.Failure("failed.to.add.user.to.role", "Failed to add user to role").ToErrorsList();
            }

            var volunteerAccount = new VolunteerAccount(user);
            await _volunteerAccountManager.CreateVolunteerAccount(volunteerAccount);

            transaction.Commit();

            _logger.LogInformation("Created volunteer account for user with id = {userId}", command.UserId);

            return volunteerAccount.Id;
        }
    }
}
