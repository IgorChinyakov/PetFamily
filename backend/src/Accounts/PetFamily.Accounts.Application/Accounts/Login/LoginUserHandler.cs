using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Accounts.Login;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Accounts.Login
{
    public class LoginUserHandler : ICommandHandler<string, LoginUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginUserHandler> _logger;
        private readonly ITokenProvider _tokenProvider;

        public LoginUserHandler(
            UserManager<User> userManager,
            ILogger<LoginUserHandler> logger,
            ITokenProvider provider)
        {
            _userManager = userManager;
            _logger = logger;
            _tokenProvider = provider;
        }

        public async Task<Result<string, ErrorsList>> Handle(
            LoginUserCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                return Errors.User.InvalidCredentials().ToErrorsList();

            var isValidPassword = await _userManager.CheckPasswordAsync(user, command.Password);
            if (!isValidPassword)
                return Errors.User.InvalidCredentials().ToErrorsList();

            var token = _tokenProvider.GenerateAccessToken(user, cancellationToken);

            return token;
        }
    }
}
