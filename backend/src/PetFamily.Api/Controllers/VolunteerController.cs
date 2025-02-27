using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Domain.Shared;

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
            var result = await handler.Handle(request, token);

            if (result.IsFailure)
                return result.Error.ToResponse();
            
            return Ok(result.Value);
        }
    }
}
