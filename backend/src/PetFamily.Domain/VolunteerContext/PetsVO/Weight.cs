using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.PetsVO
{
    public record Weight
    {
        private Weight(float value)
        {
            Value = value;
        }

        public float Value { get; }

        public static Result<Weight, Error> Create(float value)
        {
            if (value <= 0)
                return Errors.General.ValueIsInvalid("Weight");

            return new Weight(value);
        }
    }
}
