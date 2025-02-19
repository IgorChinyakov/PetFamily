using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record HealthInformation
    {
        public string Value { get; }

        private HealthInformation(string value)
        {
            Value = value;
        }

        public static Result<HealthInformation> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<HealthInformation>("Health information is not supposed to be empty");

            return new HealthInformation(value);
        }
    }
}
