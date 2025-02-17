using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetFamily.Domain.Volunteers.Value_objects
{
    public record Experience
    {
        public int Value { get; }

        private Experience(int value)
        {
            Value = value;
        }

        public static Result<Experience> Create(int value)
        {
            if(value <= 0)
                return Result.Failure<Experience>("Experience must be more than zero");

            return new Experience(value);
        }
    }
}
