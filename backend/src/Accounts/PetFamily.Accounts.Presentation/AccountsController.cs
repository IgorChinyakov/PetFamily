using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Accounts.Login;
using PetFamily.Accounts.Application.Accounts.Register;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Presentation
{
    public class AccountsController : ApplicationController
    {
        [HttpPost("registration")]
        public async Task<ActionResult> Register(
            [FromBody] RegisterUserRequest request,
            [FromServices] ICommandHandler<RegisterUserCommand> handler)
        {
            var result = await handler
                .Handle(new RegisterUserCommand(request.Email, request.Password, request.UserName));
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(
            [FromBody] LoginUserRequest request,
            [FromServices] ICommandHandler<string, LoginUserCommand> handler)
        {
            var result = await handler
                .Handle(new LoginUserCommand(request.Email, request.Password));
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }
    }
}
