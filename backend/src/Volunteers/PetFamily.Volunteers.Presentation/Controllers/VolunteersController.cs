using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Core.Options;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Framework.Processors;
using PetFamily.Volunteers.Application.Pets.Commands.ChooseMainPhoto;
using PetFamily.Volunteers.Application.Pets.Commands.Create;
using PetFamily.Volunteers.Application.Pets.Commands.Delete;
using PetFamily.Volunteers.Application.Pets.Commands.Move;
using PetFamily.Volunteers.Application.Pets.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Application.Pets.Commands.UpdateStatus;
using PetFamily.Volunteers.Application.Pets.Commands.UploadPhotos;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;
using PetFamily.Volunteers.Application.Volunteers.Commands.Delete;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteerById;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.Requests.Pets;
using PetFamily.Volunteers.Contracts.Requests.Volunteers;

namespace PetFamily.Volunteers.Presentation.Controllers
{
    public class VolunteersController : ApplicationController
    {
        public const string BUCKET_NAME = "photos";

        [Permission(Permissions.Volunteer.GET)]
        [HttpGet]
        public async Task<ActionResult> Get(
            [FromQuery] GetVolunteersWithPaginationRequest request,
            [FromServices] IQueryHandler<
                PagedList<VolunteerDto>,
                GetVolunteersWithPaginationQuery> handler,
            CancellationToken token = default)
        {
            var query = new GetVolunteersWithPaginationQuery(request.Page, request.PageSize);

            var response = await handler.Handle(query, token);

            return Ok(Envelope.Ok(response));
        }

        [Permission(Permissions.Volunteer.GET)]
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

        [Permission(Permissions.Volunteer.GET)]
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

        [Permission(Permissions.Volunteer.CREATE)]
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromServices] ICommandHandler<Guid, CreateVolunteerCommand> handler,
            [FromBody] CreateVolunteerRequest request,
            CancellationToken token = default)
        {
            var command = new CreateVolunteerCommand(
                request.FullName,
                request.Email,
                request.Description,
                request.Experience,
                request.PhoneNumber);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [Permission(Permissions.Volunteer.UPDATE)]
        [HttpPut("{id:guid}/main-info")]
        public async Task<ActionResult> UpdateMainInfo(
            [FromServices] ICommandHandler<Guid, UpdateVolunteerMainInfoCommand> handler,
            [FromBody] UpdateVolunteerMainInfoRequest request,
            [FromRoute] Guid volunteerId,
            CancellationToken token = default)
        {
            var command = new UpdateVolunteerMainInfoCommand(
                volunteerId,
                request.FullName,
                request.Email,
                request.Description,
                request.Experience,
                request.PhoneNumber);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        //[HttpPut("{id:guid}/social-media")]
        //public async Task<ActionResult> UpdateSocialMedia(
        //    [FromServices] ICommandHandler<Guid, UpdateVolunteerSocialMediaCommand> handler,
        //    [FromBody] UpdateVolunteerSocialMediaRequest request,
        //    [FromRoute] Guid id,
        //    CancellationToken token = default)
        //{
        //    var command = new UpdateVolunteerSocialMediaCommand(
        //        id,
        //         request.SocialMedia);

        //    var result = await handler.Handle(command, token);
        //    if (result.IsFailure)
        //        return result.Error.ToResponse();

        //    return Ok(Envelope.Ok(result.Value));
        //}

        //[HttpPut("{id:guid}/details")]
        //public async Task<ActionResult> UpdateDetails(
        //    [FromServices] ICommandHandler<Guid, UpdateVolunteerDetailsCommand> handler,
        //    [FromBody] UpdateVolunteerDetailsRequest request,
        //    [FromRoute] Guid id,
        //    CancellationToken token = default)
        //{
        //    var command = new UpdateVolunteerDetailsCommand(id, request.Details);

        //    var result = await handler.Handle(command, token);
        //    if (result.IsFailure)
        //        return result.Error.ToResponse();

        //    return Ok(Envelope.Ok(result.Value));
        //}

        [Permission(Permissions.Volunteer.DELETE)]
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

        [Permission(Permissions.Pet.CREATE)]
        [HttpPost("{volunteerId:guid}/pets")]
        public async Task<ActionResult> CreatePet(
            [FromServices] ICommandHandler<Guid, CreatePetCommand> handler,
            [FromRoute] Guid volunteerId,
            [FromBody] CreatePetRequest request,
            CancellationToken token = default)
        {
            var command = new CreatePetCommand(
                volunteerId,
                request.NickName,
                request.Description,
                request.SpeciesId,
                request.BreedId,
                request.Color,
                request.IsSterilized,
                request.IsVaccinated,
                request.HealthInformation,
                request.Address,
                request.Weight,
                request.Height,
                request.Birthday,
                request.PetStatus,
                request.Details);

            var result = await handler.Handle(command, token);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [Permission(Permissions.Volunteer.UPDATE)]
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

        [Permission(Permissions.Pet.UPDATE)]
        [HttpPost("{volunteerId:guid}/pets/{petId:guid}/pet-movement")]
        public async Task<ActionResult> MovePet(
            [FromServices] ICommandHandler<MovePetCommand> handler,
            [FromBody] MovePetRequest request,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            CancellationToken token = default)
        {
            var command = new MovePetCommand(volunteerId, petId, request.Position);

            var movementResult = await handler.Handle(command, token);
            if (movementResult.IsFailure)
                return movementResult.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [Permission(Permissions.Pet.UPDATE)]
        [HttpPut("{volunteerId:guid}/pets/{petId:guid}/main-info")]
        public async Task<ActionResult> UpdatePetMainInfo(
            [FromServices] ICommandHandler<UpdatePetMainInfoCommand> handler,
            [FromBody] UpdatePetMainInfoRequest request,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            CancellationToken token = default)
        {
            var command = new UpdatePetMainInfoCommand(
                petId,
                volunteerId,
                request.SpeciesId,
                request.BreedId,
                request.NickName,
                request.Address,
                request.Color,
                request.HealthInformation,
                request.Description,
                request.PhoneNumber,
                request.Height,
                request.Weight,
                request.IsSterilized,
                request.IsVaccinated,
                request.Birthday,
                request.CreationDate);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [Permission(Permissions.Pet.GET)]
        [HttpPut("{volunteerId:guid}/pets/{petId:guid}/status")]
        public async Task<ActionResult> UpdatePetStatus(
            [FromServices] ICommandHandler<UpdatePetStatusCommand> handler,
            [FromBody] UpdatePetStatusRequest request,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            CancellationToken token = default)
        {
            var command = new UpdatePetStatusCommand(volunteerId, petId, request.Status);

            var result = await handler.Handle(command, token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok());
        }

        [Permission(Permissions.Pet.DELETE)]
        [HttpDelete("{volunteerId:guid}/pets/{petId:guid}")]
        public async Task<ActionResult> DeletePet(
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

        [Permission(Permissions.Pet.UPDATE)]
        [HttpPost("{volunteerId:guid}/pets/{petId:guid}/main-photo")]
        public async Task<ActionResult> ChoosePetMainPhoto(
            [FromServices] ICommandHandler<string, ChoosePetMainPhotoCommand> handler,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] ChoosePetMainPhotoRequest request,
            CancellationToken token = default)
        {
            var command = new ChoosePetMainPhotoCommand(volunteerId, petId, request.Path);

            var result = await handler.Handle(command, token);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }
    }
}
