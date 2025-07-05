using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.VolunteerRequests.Application.Commands.CreateVolunteerRequest;
using PetFamily.VolunteerRequests.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Presentation.Controllers
{
    public class VolunteerRequestsController
        : ApplicationController
    {
        [HttpPost("{userId:guid}")]
        public async Task<ActionResult> Create(
            [FromServices] ICommandHandler<Guid, CreateVolunteerRequestCommand> handler,
            [FromRoute]Guid userId,
            [FromBody] CreateVolunteerRequestRequest request)
        {
            var command = new CreateVolunteerRequestCommand(userId, request.VolunteerInformation);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }
    }
}
