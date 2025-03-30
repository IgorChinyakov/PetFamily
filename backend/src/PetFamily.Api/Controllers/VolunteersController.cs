using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Api.Requests.Pets;
using PetFamily.Api.Requests.Volunteers;
using PetFamily.Api.Response;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Pets.Create;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.UpdateDetails;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialMedia;
using PetFamily.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PetFamily.Api.Controllers
{
    public class VolunteersController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateVolunteerHandler handler,
            [FromBody] CreateVolunteerRequest request,
            CancellationToken token = default)
        {
            var command = new CreateVolunteerCommand(
                request.FullName,
                request.Email,
                request.Description,
                request.Experience,
                request.PhoneNumber,
                request.DetailsList,
                request.SocialMediaList);

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
            var command = new UpdateMainInfoCommand(
                id, 
                request.FullName, 
                request.Email, 
                request.Description, 
                request.Experience, 
                request.PhoneNumber);

            var result = await handler.Handle(command, token);

            if(result.IsFailure)
                return result.Error.ToResponse();

            return result.Value;
        }

        [HttpPut("{id:guid}/social-media")]
        public async Task<ActionResult<Guid>> UpdateSocialMedia(
            [FromServices] UpdateSocialMediaHandler handler,
            [FromBody] UpdateSocialMediaRequest request,
            [FromRoute] Guid id,
            CancellationToken token = default)
        {
            var command = new UpdateSocialMediaCommand(id, request.SocialMedia);
                
            var result = await handler.Handle(command, token);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return result.Value;
        }

        [HttpPut("{id:guid}/details")]
        public async Task<ActionResult<Guid>> UpdateDetails(
            [FromServices] UpdateDetailsHandler handler,
            [FromBody] UpdateDetailsRequest request,
            [FromRoute] Guid id,
            CancellationToken token = default)
        {
            var command = new UpdateDetailsCommand(id, request.Details);

            var result = await handler.Handle(command, token);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return result.Value;
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

            return result.Value;
        }

        [HttpPost("{volunteerId:guid}/pets")]
        public async Task<ActionResult<Guid>> CreatePet(
            [FromServices]CreatePetHandler handler,
            [FromRoute]Guid volunteerId,
            [FromBody]CreatePetRequest request,
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
                request.PetStatus);

            var result = await handler.Handle(command, token);

            if(result.IsFailure)
                return result.Error.ToResponse();

           return result.Value;
        }

        [HttpPost("{volunteerId:guid}/pets/{petId:guid}")]
        public async Task<ActionResult<Guid>> AddPetPhotos(
            [FromServices] CreatePetHandler handler,
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection files,
            CancellationToken token = default)
        {

        }
    }
}
