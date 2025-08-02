using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Framework.Processors
{
    public class UserScopedDataProccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserScopedDataProccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Result<UserScopedData, Error> Process()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new Exception();

            var idClaim = _httpContextAccessor
                .HttpContext
                .User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idClaim == null)
                return Errors.General.ValueIsInvalid("UserId");

            var emailClaim = _httpContextAccessor
                .HttpContext
                .User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null)
                return Errors.General.ValueIsInvalid("Email");

            var userIdParseReult = Guid.TryParse(idClaim.Value, out var userId);
            if (userIdParseReult != true)
                return Errors.General.ValueIsInvalid("UserId");

            var scopedData = new UserScopedData()
            {
                Email = emailClaim.Value,
                Id = userId
            };

            return scopedData;
        }
    }
}
