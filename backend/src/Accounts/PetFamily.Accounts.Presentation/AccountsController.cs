using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Accounts.Login;
using PetFamily.Accounts.Application.Accounts.Register;
using PetFamily.Accounts.Application.Features.UpdateDetails;
using PetFamily.Accounts.Application.Features.UpdateSocialMedia;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
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
                .Handle(new RegisterUserCommand(
                    request.Email, 
                    request.Password, 
                    request.UserName, 
                    request.FirstName,
                    request.SecondName,
                    request.LastName));
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

        [Permission(Permissions.UPDATE)]
        [HttpPut("{id:guid}/social-media")]
        public async Task<ActionResult> UpdateSocialMedia(
            [FromServices] ICommandHandler<Guid, UpdateUserSocialMediaCommand> handler,
            [FromBody] UpdateUserSocialMediaRequest request,
            [FromRoute] Guid id,
            CancellationToken token = default)
        {
            var command = new UpdateUserSocialMediaCommand(id, request.SocialMedia);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [Permission(Permissions.VolunteerAccount.UPDATE)]
        [HttpPut("{id:guid}/details")]
        public async Task<ActionResult> UpdateDetails(
            [FromServices] ICommandHandler<Guid, UpdateUserDetailsCommand> handler,
            [FromBody] UpdateVolunteerDetailsRequest request,
            [FromRoute] Guid id,
            CancellationToken token = default)
        {
            var command = new UpdateUserDetailsCommand(id, request.Details);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }
    }
}
