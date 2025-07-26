using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.VolunteerRequests.Application.Features.Commands.Approve;
using PetFamily.VolunteerRequests.Application.Features.Commands.Create;
using PetFamily.VolunteerRequests.Application.Features.Commands.Reject;
using PetFamily.VolunteerRequests.Application.Features.Commands.SendForRevision;
using PetFamily.VolunteerRequests.Application.Features.Commands.TakeOnReview;
using PetFamily.VolunteerRequests.Application.Features.Commands.Update;
using PetFamily.VolunteerRequests.Application.Features.Queries.GetRequestsByAdminId;
using PetFamily.VolunteerRequests.Application.Features.Queries.GetRequestsByuserId;
using PetFamily.VolunteerRequests.Application.Features.Queries.GetSubmittedWithPagination;
using PetFamily.VolunteerRequests.Contracts.DTOs;
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
        [HttpPut("{requestId:guid}/on-review")]
        public async Task<ActionResult> TakeOnReview(
            [FromServices] ICommandHandler<TakeRequestOnReviewCommand> handler,
            [FromRoute] Guid requestId)
        {
            var command = new TakeRequestOnReviewCommand(requestId, GetUserId().Value);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
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

        [Permission(Permissions.VolunteerRequest.UPDATE)]
        [HttpPut("{requestId:guid}")]
        public async Task<ActionResult> Update(
            [FromServices] ICommandHandler<UpdateRequestCommand> handler,
            [FromRoute] Guid requestId,
            [FromBody] UpdateVolunteerRequestRequest request)
        {
            var command = new UpdateRequestCommand(
                requestId,
                GetUserId().Value,
                request.UpdatedInformation);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [Permission(Permissions.VolunteerRequest.GET)]
        [HttpGet("submitted-requests")]
        public async Task<ActionResult> GetSubmittedRequests(
            [FromServices] IQueryHandler<PagedList<VolunteerRequestDto>, GetSubmittedRequestsQuery> handler,
            [FromBody] GetSubmittedRequestsRequest request)
        {
            var query = new GetSubmittedRequestsQuery(
                request.Page,
                request.PageSize);

            var result = await handler.Handle(query);

            return Ok(Envelope.Ok(result));
        }

        [Permission(Permissions.VolunteerRequest.GET)]
        [HttpGet("admin-requests")]
        public async Task<ActionResult> GetRequestsByAdminId(
            [FromServices] IQueryHandler<PagedList<VolunteerRequestDto>, GetRequestsByAdminIdQuery> handler,
            [FromRoute] Guid requestId,
            [FromBody] GetRequestsByAdminIdRequest request)
        {
            var query = new GetRequestsByAdminIdQuery(
                GetUserId().Value,
                request.Page,
                request.PageSize,
                request.Status);

            var result = await handler.Handle(query);

            return Ok(Envelope.Ok(result));
        }

        [Permission(Permissions.VolunteerRequest.GET_BY_USER_ID)]
        [HttpGet("users-requests")]
        public async Task<ActionResult> GetRequestsByUserId(
            [FromServices] IQueryHandler<PagedList<VolunteerRequestDto>, GetRequestsByUserIdQuery> handler,
            [FromRoute] Guid userId,
            [FromBody] GetRequestsByUserIdRequest request)
        {
            var query = new GetRequestsByUserIdQuery(
                GetUserId().Value,
                request.Page,
                request.PageSize,
                request.Status);

            var result = await handler.Handle(query);

            return Ok(Envelope.Ok(result));
        }
    }
}
