using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetFamily.Domain.Volunteers.Value_objects
{
    public record Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<Email>("Email is not supposed to be empty");

            if(Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result.Failure<Email>("Email is not correct");

            return new Email(value);
        }
    }
}
