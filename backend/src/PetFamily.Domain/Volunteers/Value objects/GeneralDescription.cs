using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetFamily.Domain.Volunteers.Value_objects
{
    public record GeneralDescription
    {
        public string Value { get; }

        private GeneralDescription(string value)
        {
            Value = value;
        }

        public static Result<GeneralDescription> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<GeneralDescription>("Description is not supposed to be empty");

            return new GeneralDescription(value);
        }
    }
}
