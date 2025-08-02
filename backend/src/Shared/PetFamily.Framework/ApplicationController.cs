using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;
using System.Security.Claims;

namespace PetFamily.Framework
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        public const string USER_SCOPED_DATA_KEY = "UserScopedData";

        protected UserScopedData? UserScopedData => HttpContext.Items[USER_SCOPED_DATA_KEY] as UserScopedData;
    }
}
