using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Domain.PetsVO
{
    public record Color
    {
        public string Value { get; }

        private Color(string value)
        {
            Value = value;
        }

        public static Result<Color, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_LOW_TITLE_LENGTH)
                return Errors.General.ValueIsInvalid("Color");

            return new Color(value);
        }
    }
}
