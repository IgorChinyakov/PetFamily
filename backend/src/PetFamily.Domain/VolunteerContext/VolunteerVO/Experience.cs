using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO
{
    public record Experience
    {
        public int Value { get; }

        private Experience(int value)
        {
            Value = value;
        }

        public static Result<Experience, Error> Create(int value)
        {
            if (value <= 0)
                return Errors.General.ValueIsInvalid("Experience");

            return new Experience(value);
        }
    }
}
