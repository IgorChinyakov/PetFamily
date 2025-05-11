using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
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
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(
            UserManager<User> userManager, 
            ILogger<RegisterUserHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            RegisterUserCommand command, CancellationToken cancellationToken = default)
        {
            var user = new User
            {
                Email = command.Email,
                UserName = command.UserName,
            };

            var result = await _userManager.CreateAsync(user, command.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User with username: {UserName} has been created", command.UserName);
                return Result.Success<ErrorsList>();
            }

            _logger.LogInformation("Failed to create user with username: {UserName}", command.UserName);
            var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description));

            return new ErrorsList(errors);
        }
    }
}
