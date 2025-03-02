using CSharpFunctionalExtensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Response;
using PetFamily.Domain.Shared;
using System.Runtime.CompilerServices;

namespace PetFamily.Api.Extensions
{
    public static class ResponseExtensions
    {
        public static ActionResult ToResponse(this Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            var responseError = new ResponseError(error.Code, error.Message, null);

            var envelope = Envelope.Error([responseError]);

            return new ObjectResult(envelope)
            {
                StatusCode = statusCode
            };
        }

        public static ActionResult ToValidationErrorResponse(this ValidationResult result)
        {
            if (result.IsValid)
                throw new InvalidOperationException("Result is not supposed to be succeed");

            var validationErrors = result.Errors;

            var responseErrors = from validationError in validationErrors
                                 let errorMessage = validationError.ErrorMessage
                                 let error = Error.Deserialize(errorMessage)
                                 select new ResponseError(error.Code, error.Message, error.PropertyName);

            var envelope = Envelope.Error(responseErrors);

            return new ObjectResult(envelope)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }
}
