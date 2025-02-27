using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets.Value_objects;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.SpeciesContext.ValueObjects
{
    public record Name
    {
        public const int MAX_LENGTH = 40;
        public string Value { get; }

        private Name(string value)
        {
            Value = value;
        }

        public static Result<Name, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
                return Errors.General.ValueIsInvalid("Name");

            return new Name(value);
        }
    }
}
