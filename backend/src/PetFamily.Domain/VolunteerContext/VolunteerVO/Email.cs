using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO
{
    public record Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) 
                || !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$") 
                || value.Length > Constants.MAX_LOW_TITLE_LENGTH)
                return Errors.General.ValueIsInvalid("Email");

            return new Email(value);
        }
    }
}
