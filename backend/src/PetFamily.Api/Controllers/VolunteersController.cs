using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Api.Processors;
using PetFamily.Api.Requests.Pets;
using PetFamily.Api.Requests.Volunteers;
using PetFamily.Api.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs;
using PetFamily.Application.EntitiesHandling;
using PetFamily.Application.EntitiesHandling.Pets.Commands.Create;
using PetFamily.Application.EntitiesHandling.Pets.Commands.Move;
using PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateMainInfo;
using PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateStatus;
using PetFamily.Application.EntitiesHandling.Pets.Commands.UploadPhotos;
using PetFamily.Application.EntitiesHandling.Pets.Queries.GetById;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Create;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Delete;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateDetails;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateSocialMedia;
using PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteerById;
using PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Api.Controllers
{
    public class VolunteersController : ApplicationController
    {
        public const string BUCKET_NAME = "photos";

        [HttpGet]
        public async Task<ActionResult> Get(
            [FromQuery] GetVolunteersWithPaginationRequest request,
            [FromServices] IQueryHandler<
                PagedList<VolunteerDto>, 
                GetVolunteersWithPaginationQuery> handler,
            CancellationToken token = default)
        {
            var query = request.ToQuery();

            var response = await handler.Handle(query, token);

            return Ok(Envelope.Ok(response));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetById(
            [FromRoute] Guid id,
            [FromServices] IQueryHandlerWithResult<VolunteerDto, GetVolunteerByIdQuery> handler,
            CancellationToken token = default)
        {
            var query = new GetVolunteerByIdQuery(id);
            var response = await handler.Handle(query, token);
            if (response.IsFailure)
                return response.Error.ToResponse();

            return Ok(Envelope.Ok(response));
        }

        [HttpGet("dapper")]
        public async Task<ActionResult> GetWithDapper(
            [FromQuery] GetVolunteersWithPaginationRequest request,
            [FromServices] IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQueryWithDapper> handler,
            CancellationToken token = default)
        {
            var query = new GetVolunteersWithPaginationQueryWithDapper(request.Page, request.PageSize);

            var response = await handler.Handle(query, token);

            return Ok(Envelope.Ok(response));
        }

        [HttpPost]
        public async Task<ActionResult> Create(
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
        public async Task<ActionResult> UpdateMainInfo(
            [FromServices] ICommandHandler<Guid, UpdateVolunteerMainInfoCommand> handler,
            [FromBody] UpdateVolunteerMainInfoRequest request,
            [FromRoute] Guid volunteerId,
            CancellationToken token = default)
        {
            var command = request.ToCommand(volunteerId);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPut("{id:guid}/social-media")]
        public async Task<ActionResult> UpdateSocialMedia(
            [FromServices] ICommandHandler<Guid, UpdateVolunteerSocialMediaCommand> handler,
            [FromBody] UpdateVolunteerSocialMediaRequest request,
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
        public async Task<ActionResult> UpdateDetails(
            [FromServices] ICommandHandler<Guid, UpdateVolunteerDetailsCommand> handler,
            [FromBody] UpdateVolunteerDetailsRequest request,
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
        public async Task<ActionResult> Delete(
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
        public async Task<ActionResult> CreatePet(
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
        public async Task<ActionResult> AddPetPhotos(
            [FromServices] ICommandHandler<IReadOnlyList<string>, UploadPetPhotosCommand> handler,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection files,
            CancellationToken token = default)
        {
            await using var processor = new FormFileProcessor();
            var fileDtos = processor.Process(files);
            var command = new UploadPetPhotosCommand(
                fileDtos, 
                volunteerId, 
                petId, 
                BUCKET_NAME);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost("{volunteerId:guid}/pets/{petId:guid}/pet-movement")]
        public async Task<ActionResult> MovePet(
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

            return Ok(Envelope.Ok());
        }

        [HttpPut("{volunteerId:guid}/pets/{petId:guid}/main-info")]
        public async Task<ActionResult> UpdatePetMainInfo(
            [FromServices] ICommandHandler<UpdatePetMainInfoCommand> handler,
            [FromBody] UpdatePetMainInfoRequest request,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            CancellationToken token = default)
        {
            var command = request.ToCommand(petId, volunteerId);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [HttpPut("{volunteerId:guid}/pets/{petId:guid}/status")]
        public async Task<ActionResult> UpdatePetStatus(
            [FromServices] ICommandHandler<UpdatePetStatusCommand> handler,
            [FromBody] UpdatePetStatusRequest request,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            CancellationToken token = default)
        {
            var command = request.ToCommand(petId, volunteerId);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [HttpDelete("{volunteerId:guid}/pets/{petId:guid}")]
        public async Task<ActionResult> Delete(
            [FromServices] ICommandHandler<Guid, DeletePetCommand> handler,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] DeletionOptions options,
            CancellationToken token = default)
        {
            var command = new DeletePetCommand(
                volunteerId, 
                petId, 
                options, 
                BUCKET_NAME);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost("{volunteerId:guid}/pets/{petId:guid}/main-photo")]
        public async Task<ActionResult> ChoosePetMainPhoto(
            [FromServices] ICommandHandler<string, ChoosePetMainPhotoCommand> handler,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] ChoosePetMainPhotoRequest request,
            CancellationToken token = default)
        {
            var command = request.ToCommand(volunteerId, petId);

            var result = await handler.Handle(command, token);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }
    }
}
