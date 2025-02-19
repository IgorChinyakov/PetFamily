using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record CreationDate
    {
        public DateTime Value { get; }

        private CreationDate(DateTime value)
        {
            Value = value;
        }

        public static Result<CreationDate> Create(DateTime value)
        {
            if (value > DateTime.UtcNow)
                return Result.Failure<CreationDate>("Incorrct date");
            if (DateTime.Compare(value, new DateTime(2000, 1, 1)) < 0)
                return Result.Failure<CreationDate>("Date is too old");

            return new CreationDate(value);
        }
    }
}
