using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record Color
    {
        public string Value { get; }

        private Color(string value)
        {
            Value = value;
        }

        public static Result<Color> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<Color>("Color is not supposed to be empty");

            return new Color(value);
        }
    }
}
