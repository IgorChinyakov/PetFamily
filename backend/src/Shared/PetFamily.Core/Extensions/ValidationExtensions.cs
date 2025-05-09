using FluentValidation.Results;
using PetFamily.SharedKernel;

namespace PetFamily.Core.Extensions
{
    public static class ValidationExtensions
    {
        public static ErrorsList ToErrorsList(this ValidationResult result)
        {
            var errors = result.Errors.Select(e => Error.Deserialize(e.ErrorMessage)).ToList();
            return new ErrorsList(errors);
        }
    }
}
