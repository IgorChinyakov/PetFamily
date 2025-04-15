using FluentValidation.Results;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Extensions
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
