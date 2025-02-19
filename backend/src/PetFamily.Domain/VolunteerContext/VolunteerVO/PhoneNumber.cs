using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets.Value_objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO
{
    public record PhoneNumber
    {
        public string Value { get; set; }

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public Result<PhoneNumber> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<PhoneNumber>("Number is not supposed to be empty");

            if (!Regex.IsMatch(value, @"^(\+7|8)?[\s\-]?\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}$"))
                return Result.Failure<PhoneNumber>("Number is not correct");

            return new PhoneNumber(value);
        }
    }
}
