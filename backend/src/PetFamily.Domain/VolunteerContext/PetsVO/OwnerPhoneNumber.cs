using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record OwnerPhoneNumber
    {
        public string Value { get; set; }

        private OwnerPhoneNumber(string value)
        {
            Value = value;
        }

        public Result<OwnerPhoneNumber> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<OwnerPhoneNumber>("Number is not supposed to be empty");

            if(!Regex.IsMatch(value, @"^(\+7|8)?[\s\-]?\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}$"))
                return Result.Failure<OwnerPhoneNumber>("Number is not correct");

            return new OwnerPhoneNumber(value);
        }
    }
}
