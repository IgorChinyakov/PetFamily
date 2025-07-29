using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Domain.ValueObjects.Message
{
    public class Text : ValueObject
    {
        private Text(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Text, Error> Create(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid("text");

            return new Text(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
