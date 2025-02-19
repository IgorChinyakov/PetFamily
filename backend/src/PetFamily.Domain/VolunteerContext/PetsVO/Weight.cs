using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record Weight
    {
        private Weight(float value)
        {
            Value = value;
        }

        public float Value { get; }

        public static Result<Weight> Create(float value)
        {
            if (value <= 0)
                return Result.Failure<Weight>("Weight must be more than zero");

            return new Weight(value);
        }
    }
}
