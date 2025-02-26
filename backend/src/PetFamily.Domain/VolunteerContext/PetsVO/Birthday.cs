using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
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
            if (value > DateTime.UtcNow || value < new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                return Errors.General.ValueIsInvalid("Date");

            return new Birthday(value);
        }
    }
}
