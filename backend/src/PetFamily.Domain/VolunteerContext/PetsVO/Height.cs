using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record Height
    {
        public float Value { get; }

        private Height(float value)
        {
            Value = value;
        }

        public static Result<Height> Create(float value)
        {
            if (value <= 0)
                return Result.Failure<Height>("Height must be more than zero");

            return new Height(value);
        }
    }
}
