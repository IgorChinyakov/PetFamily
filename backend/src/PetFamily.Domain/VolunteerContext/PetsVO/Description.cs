using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record Description
    {
        public string Value { get; }

        private Description(string value)
        {
            Value = value;
        }

        public static Result<Description> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<Description>("Description is not supposed to be empty");

            return new Description(value);
        }
    }
}
