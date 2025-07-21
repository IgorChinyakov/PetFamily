using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects
{
    public class RejectionComment : ValueObject
    {
        public const int MAX_LENGTH = 1500;

        public string Value { get; }

        private RejectionComment(string value)
        {
            Value = value;
        }

        public static Result<RejectionComment, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
                return Errors.General.ValueIsInvalid("Rejection comment");

            return new RejectionComment(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
