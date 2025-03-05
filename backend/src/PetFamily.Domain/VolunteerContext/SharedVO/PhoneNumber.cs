using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets.Value_objects;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.SharedVO
{
    public record PhoneNumber
    {
        public string Value { get; }

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public static Result<PhoneNumber, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) 
                || !Regex.IsMatch(value, @"^(\+7|8)?[\s\-]?\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}$") 
                || value.Length > Constants.MAX_LOW_TITLE_LENGTH)
                return Errors.General.ValueIsInvalid("Number");

            return new PhoneNumber(value);
        }
    }
}
