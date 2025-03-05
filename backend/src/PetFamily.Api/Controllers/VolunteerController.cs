using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Contracts;
using PetFamily.Api.Extensions;
using PetFamily.Api.Response;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PetFamily.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VolunteerController : ControllerBase
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
    }
}
