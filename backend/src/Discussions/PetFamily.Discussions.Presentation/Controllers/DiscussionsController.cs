using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Discussions.Application.Features.Commands.AddMessage;
using PetFamily.Discussions.Application.Features.Commands.Close;
using PetFamily.Discussions.Application.Features.Commands.Create;
using PetFamily.Discussions.Application.Features.Commands.EditMessage;
using PetFamily.Discussions.Application.Features.Commands.RemoveMessage;
using PetFamily.Discussions.Application.Features.Queries.GetDiscussionByRelationId;
using PetFamily.Discussions.Contracts.DTOs;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Framework.Filters;
using PetFamily.Framework.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Presentation.Controllers
{
    [ServiceFilter(typeof(UserScopedDataFilter))]
    public class DiscussionsController : ApplicationController
    {
        [Permission(Permissions.Discussions.UPDATE)]
        [HttpPut("{discussionId:guid}")]
        public async Task<ActionResult> Close(
            [FromServices] ICommandHandler<CloseDiscussionCommand> handler,
            [FromRoute] Guid discussionId)
        {
            var command = new CloseDiscussionCommand(discussionId);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [Permission(Permissions.Messages.CREATE)]
        [HttpPost("{discussionId:guid}")]
        public async Task<ActionResult> AddMessage(
            [FromServices] ICommandHandler<AddMessageToDiscussionCommand> handler,
            [FromRoute] Guid discussionId,
            [FromBody] AddMessageToDiscussionRequest request)
        {
            var command = new AddMessageToDiscussionCommand(
                discussionId,
                UserScopedData!.Id!.Value, 
                request.Text);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [Permission(Permissions.Messages.UPDATE)]
        [HttpPut("{discussionId:guid}/messages/{messageId:guid}")]
        public async Task<ActionResult> EditMessage(
            [FromServices] ICommandHandler<EditMessageCommand> handler,
            [FromRoute] Guid discussionId,
            [FromRoute] Guid messageId,
            [FromBody] EditMessageRequest request)
        {
            var command = new EditMessageCommand(
                discussionId,
                messageId,
                UserScopedData!.Id!.Value,
                request.EditedMessage);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [Permission(Permissions.Messages.DELETE)]
        [HttpDelete("{discussionId:guid}/messages/{messageId:guid}")]
        public async Task<ActionResult> RemoveMessage(
            [FromServices] ICommandHandler<RemoveMessageCommand> handler,
            [FromRoute] Guid discussionId,
            [FromRoute] Guid messageId)
        {
            var command = new RemoveMessageCommand(
                discussionId,
                messageId,
                UserScopedData!.Id!.Value);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [Permission(Permissions.Discussions.GET)]
        [HttpGet("{relationId:guid}")]
        public async Task<ActionResult> Get(
            [FromServices] IQueryHandlerWithResult<DiscussionDto, GetDiscussionByRelationIdQuery> handler,
            [FromRoute] Guid relationId)
        {
            var command = new GetDiscussionByRelationIdQuery(relationId);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }
    }
}
