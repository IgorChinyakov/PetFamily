using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Domain.ValueObjects;
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
            var user = User.CreateParticipant(
                command.UserName,
                command.Email,
                new FullName
                {
                    Name = command.FirstName,
                    SecondName = command.SecondName,
                    FamilyName = command.LastName
                });
            var user = new User
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = command.Email,
                UserName = command.UserName,
                FullName = new FullName
                {
                    Name = command.FirstName,
                    SecondName = command.SecondName,
                    FamilyName = command.LastName
                }
            };

            var result = await _userManager.CreateAsync(user, command.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User with username: {UserName} has been created", command.UserName);
                await _userManager.AddToRoleAsync(user, "Participant"); 
                return Result.Success<ErrorsList>();
            }

            _logger.LogInformation("Failed to create user with username: {UserName}", command.UserName);
            var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description));

            return new ErrorsList(errors);
        }
    }
}
