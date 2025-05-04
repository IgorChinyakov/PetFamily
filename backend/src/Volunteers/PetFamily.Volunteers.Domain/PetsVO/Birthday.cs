using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Domain.PetsVO
{
    public record Birthday
    {
        public DateTime Value { get; }

        private Birthday(DateTime value)
        {
            Value = value;
        }

        public static Result<Birthday, Error> Create(DateTime value)
        {
            if (value > DateTime.UtcNow || value < new DateTime(2000, 1, 1, 0, 0, 0))
                return Errors.General.ValueIsInvalid("Date");

            return new Birthday(value);
        }
    }
}
