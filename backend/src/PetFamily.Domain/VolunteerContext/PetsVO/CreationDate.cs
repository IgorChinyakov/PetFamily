using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.PetsVO
{
    public record CreationDate
    {
        public DateTime Value { get; }

        private CreationDate(DateTime value)
        {
            Value = value;
        }

        public static Result<CreationDate, Error> Create(DateTime value)
        {
            if (value > DateTime.UtcNow || value < new DateTime(2000, 1, 1, 0, 0, 0))
                return Errors.General.ValueIsInvalid("Date");

            return new CreationDate(value);
        }
    }
}
