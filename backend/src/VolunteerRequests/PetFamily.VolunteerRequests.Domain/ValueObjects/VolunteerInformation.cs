using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects
{
    public record VolunteerInformation
    {
        public const int MAX_LENGTH = 1500;

        public string Value { get; }

        private VolunteerInformation(string value)
        {
            Value = value;
        }

        public static Result<VolunteerInformation, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
                return Errors.General.ValueIsInvalid("Volunteer information");

            return new VolunteerInformation(value);
        }
    }
}
