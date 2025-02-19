using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record NickName
    {
        public string Value { get; }

        private NickName(string value)
        {
            Value = value;
        }

        public static Result<NickName> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<NickName>("Nickname is not supposed to be empty");

            return new NickName(value);
        }
    }
}
