using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Domain.PetsVO
{
    public record Height
    {
        public float Value { get; }

        private Height(float value)
        {
            Value = value;
        }

        public static Result<Height, Error> Create(float value)
        {
            if (value <= 0)
                return Errors.General.ValueIsInvalid("Height");

            return new Height(value);
        }
    }
}
