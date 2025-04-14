using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Api.Processors;
using PetFamily.Api.Requests.Pets;
using PetFamily.Api.Requests.Volunteers;
using PetFamily.Api.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Models;
using PetFamily.Application.Pets.Commands.Create;
using PetFamily.Application.Pets.Commands.Move;
using PetFamily.Application.Pets.Commands.UploadPhotos;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Application.Volunteers.Commands.Delete;
using PetFamily.Application.Volunteers.Commands.UpdateDetails;
using PetFamily.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Application.Volunteers.Commands.UpdateSocialMedia;
using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetFamily.Api.Controllers
{
    public class VolunteersController : ApplicationController
    {
        public const string BUCKET_NAME = "photos";

        [HttpGet]
        public async Task<ActionResult<Guid>> Get(
            [FromQuery] GetWithPaginationRequest request,
            [FromServices] IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery> handler,
            CancellationToken token = default)
        {
            var query = request.ToQuery();

            var response = await handler.Handle(query, token);

            return Ok(Envelope.Ok(response));
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] ICommandHandler<Guid, CreateVolunteerCommand> handler,
            [FromBody] CreateVolunteerRequest request,
            CancellationToken token = default)
        {
            var command = request.ToCommand();

            var result = await handler.Handle(command, token);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPut("{id:guid}/main-info")]
        public async Task<ActionResult<Guid>> UpdateMainInfo(
            [FromServices] ICommandHandler<Guid, UpdateMainInfoCommand> handler,
            [FromBody] UpdateMainInfoRequest request,
            [FromRoute] Guid id,
            CancellationToken token = default)
        {
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, token);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPut("{id:guid}/social-media")]
        public async Task<ActionResult<Guid>> UpdateSocialMedia(
            [FromServices] ICommandHandler<Guid, UpdateSocialMediaCommand> handler,
            [FromBody] UpdateSocialMediaRequest request,
            [FromRoute] Guid id,
            CancellationToken token = default)
        {
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, token);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPut("{id:guid}/details")]
        public async Task<ActionResult<Guid>> UpdateDetails(
            [FromServices] ICommandHandler<Guid, UpdateDetailsCommand> handler,
            [FromBody] UpdateDetailsRequest request,
            [FromRoute] Guid id,
            CancellationToken token = default)
        {
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, token);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> Delete(
            [FromServices] ICommandHandler<Guid, DeleteVolunteerCommand> handler,
            [FromRoute] Guid id,
            [FromBody] DeletionOptions options,
            CancellationToken token = default)
        {
            var command = new DeleteVolunteerCommand(id, options);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost("{volunteerId:guid}/pets")]
        public async Task<ActionResult<Guid>> CreatePet(
            [FromServices] ICommandHandler<Guid, CreatePetCommand> handler,
            [FromRoute] Guid volunteerId,
            [FromBody] CreatePetRequest request,
            CancellationToken token = default)
        {
            var command = request.ToCommand(volunteerId);

            var result = await handler.Handle(command, token);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost("{volunteerId:guid}/pets/{petId:guid}/photos")]
        public async Task<ActionResult<Guid>> AddPetPhotos(
            [FromServices] ICommandHandler<IReadOnlyList<string>, UploadPhotosCommand> handler,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection files,
            CancellationToken token = default)
        {
            await using var processor = new FormFileProcessor();
            var fileDtos = processor.Process(files);
            var command = new UploadPhotosCommand(fileDtos, volunteerId, petId, BUCKET_NAME);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost("{volunteerId:guid}/pets/{petId:guid}/pet-movement")]
        public async Task<ActionResult<Guid>> MovePet(
            [FromServices] ICommandHandler<MovePetCommand> handler,
            [FromBody] MovePetRequest request,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            CancellationToken token = default)
        {
            var command = request.ToCommand(volunteerId, petId);

            var movementResult = await handler.Handle(command, token);
            if (movementResult.IsFailure)
                return movementResult.Error.ToResponse();

            return Ok();
        }
    }
}
