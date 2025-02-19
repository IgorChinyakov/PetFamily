using CSharpFunctionalExtensions;
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

        public static Result<Birthday> Create(DateTime value)
        {
            if (value > DateTime.UtcNow)
                return Result.Failure<Birthday>($"Date {value} can't be in the future");
            if (value < new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                return Result.Failure<Birthday>("Date is too old. Must be after 01/01/2000.");

            return new Birthday(value);
        }
    }
}
