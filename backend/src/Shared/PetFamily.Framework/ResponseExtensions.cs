using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;

namespace PetFamily.Framework
{
    public static class ResponseExtensions
    {
        public static ActionResult ToResponse(this Error error)
        {
            var statusCode = GetStatusCodeForErrorType(error.Type);

            var envelope = Envelope.Error(error.ToErrorsList());

            return new ObjectResult(envelope)
            {
                StatusCode = statusCode
            };
        }

        public static ActionResult ToResponse(this ErrorsList errors)
        {
            if (!errors.Any())
            {
                return new ObjectResult(Envelope.Error(errors))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            var distinctErrorTypes = errors.Select(e => e.Type).Distinct().ToList();

            var statusCode = distinctErrorTypes.Count() > 1 ?
                StatusCodes.Status500InternalServerError :
                GetStatusCodeForErrorType(distinctErrorTypes.First());

            return new ObjectResult(Envelope.Error(errors))
            {
                StatusCode = statusCode
            };
        }

        private static int GetStatusCodeForErrorType(ErrorType type)
            => type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                ErrorType.AccessDenied => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };
    }
}
