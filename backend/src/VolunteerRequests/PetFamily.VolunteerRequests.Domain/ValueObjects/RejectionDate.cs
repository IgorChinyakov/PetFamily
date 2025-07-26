using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects
{
    public class RejectionDate : ValueObject
    {
        public DateTime Value { get; }

        private RejectionDate(DateTime value)
        {
            Value = value;
        }

        public static Result<RejectionDate, Error> Create(DateTime value)
        {
            if (value > DateTime.UtcNow || value < new DateTime(2000, 1, 1, 0, 0, 0))
                return Errors.General.ValueIsInvalid("Date");

            return new RejectionDate(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
