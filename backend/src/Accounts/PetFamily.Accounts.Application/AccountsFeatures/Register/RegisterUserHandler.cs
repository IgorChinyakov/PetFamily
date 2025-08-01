using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Accounts.Register
{
    public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IParticipantAccountManager _participantAccountManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(
            UserManager<User> userManager,
            ILogger<RegisterUserHandler> logger,
            RoleManager<Role> roleManager,
            [FromKeyedServices(UnitOfWorkKeys.Accounts)] IUnitOfWork unitOfWork,
            IParticipantAccountManager participantAccountManager)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _participantAccountManager = participantAccountManager;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            RegisterUserCommand command, CancellationToken cancellationToken = default)
        {
            var role = await _roleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT);
            if (role == null)
                return Errors.General.ValueIsInvalid("role").ToErrorsList();

            var userResult = User.CreateParticipant(
                command.UserName,
                command.Email,
                new FullName
                {
                    Name = command.FirstName,
                    SecondName = command.SecondName,
                    FamilyName = command.LastName
                },
                role);
            if (userResult.IsFailure)
                return userResult.Error.ToErrorsList();

            var transaction = await _unitOfWork.BeginTransaction();

            var result = await _userManager.CreateAsync(userResult.Value, command.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description));
                _logger.LogInformation("Failed to create user with username: {UserName}", command.UserName);
                transaction.Rollback();
                return new ErrorsList(errors);
            }

            var participantAccount = new ParticipantAccount(userResult.Value);
            await _participantAccountManager.CreateParticipantAccount(participantAccount);

            _logger.LogInformation("User with username: {UserName} has been created", command.UserName);

            transaction.Commit();

            return Result.Success<ErrorsList>();
        }
    }
}
