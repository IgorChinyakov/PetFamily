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
        [Permission(Permissions.VolunteerRequest.CREATE)]
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromServices] ICommandHandler<Guid, CreateRequestCommand> handler,
            [FromBody] CreateVolunteerRequestRequest request)
        {
            var command = new CreateRequestCommand(GetUserId().Value, request.VolunteerInformation);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }
        [Permission(Permissions.VolunteerRequest.UPDATE_STATUS)]
        [HttpPut("{requestId:guid}/on-revision")]
        public async Task<ActionResult> SendOnRevision(
            [FromServices] ICommandHandler<SendRequestForRevisionCommand> handler,
            [FromRoute] Guid requestId,
            [FromBody] SendRequestOnRevisionRequest request)
        {
            var command = new SendRequestForRevisionCommand(
                requestId,
                GetUserId().Value,
                request.rejectionComment);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [Permission(Permissions.VolunteerRequest.UPDATE_STATUS)]
        [HttpPut("{requestId:guid}/rejection")]
        public async Task<ActionResult> Reject(
            [FromServices] ICommandHandler<RejectRequestCommand> handler,
            [FromRoute] Guid requestId)
        {
            var command = new RejectRequestCommand(GetUserId().Value, requestId);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [Permission(Permissions.VolunteerRequest.UPDATE_STATUS)]
        [HttpPut("{requestId:guid}/aprrovement")]
        public async Task<ActionResult> Approve(
            [FromServices] ICommandHandler<ApproveRequestCommand> handler,
            [FromRoute] Guid requestId)
        {
            var command = new ApproveRequestCommand(requestId, GetUserId().Value);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }
    }
}
