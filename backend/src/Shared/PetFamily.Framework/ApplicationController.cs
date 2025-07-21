using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using PetFamily.SharedKernel;
using System.Security.Claims;

namespace PetFamily.Framework
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : ControllerBase
    {
        protected Result<Guid, Error> GetUserId()
        {
            var claimId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claimId == null)
                return Errors.General.ValueIsInvalid("userId");

            var parseResult = Guid.TryParse(claimId.Value, out var userId);
            if (!parseResult)
                return Errors.General.ValueIsInvalid("userId");

            return userId;
        }
    }
}
