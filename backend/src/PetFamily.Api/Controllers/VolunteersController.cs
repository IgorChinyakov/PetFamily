using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Api.Processors;
using PetFamily.Api.Requests.Pets;
using PetFamily.Api.Requests.Volunteers;
using PetFamily.Api.Response;
using PetFamily.Application.Pets.UseCases.Create;
using PetFamily.Application.Pets.UseCases.Move;
using PetFamily.Application.Pets.UseCases.UploadPhotos;
using PetFamily.Application.Volunteers.UseCases.Create;
using PetFamily.Application.Volunteers.UseCases.Delete;
using PetFamily.Application.Volunteers.UseCases.UpdateDetails;
using PetFamily.Application.Volunteers.UseCases.UpdateMainInfo;
using PetFamily.Application.Volunteers.UseCases.UpdateSocialMedia;

namespace PetFamily.Api.Controllers
{
    public class VolunteersController : ApplicationController
    {
        public const string BUCKET_NAME = "photos";

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateVolunteerHandler handler,
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
            [FromServices] UpdateMainInfoHandler handler,
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
            [FromServices] UpdateSocialMediaHandler handler,
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
            [FromServices] UpdateDetailsHandler handler,
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
            [FromServices] DeleteVolunteerHandler handler,
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
            [FromServices] CreatePetHandler handler,
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
            [FromServices] UploadPhotosHandler handler,
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
            [FromServices] MovePetHandler handler,
            [FromBody] MovePetRequest request,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            CancellationToken token = default)
        {
            var command = request.ToCommand(volunteerId, petId);

            var movementResult = await handler.Handle(command, token);
            if(movementResult.IsFailure)
                return movementResult.Error.ToResponse();
           
            return Ok();
        }
    }
}
