using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.PetsVO
{
    public record Position
    {
        public int Value { get; }

        private Position(int value)
        {
            Value = value;
        }

        public static Result<Position, Error> Create(int value) 
        {
            if (value < 1)
                return Errors.General.ValueIsInvalid("Serial number");

            return new Position(value); 
        }

        public Result<Position, Error> Forward()
            => Create(Value + 1);

        public Result<Position, Error> Back()
            => Create(Value - 1); 

        public static implicit operator int(Position number)
            => number.Value;
    }
}
