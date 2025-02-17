using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets.Value_objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Species.ValueObjects
{
    public record Name
    {
        public string Value { get; }

        private Name(string value)
        {
            Value = value;
        }

        public static Result<Name> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<Name>("Name is not supposed to be empty");

            return new Name(value);
        }
    }
}
