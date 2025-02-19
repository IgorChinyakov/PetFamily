using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record IsSterilized
    {
        public bool Value { get; }

        private IsSterilized(bool value)
        {
            Value = value;
        }

        public static Result<IsSterilized> Create(bool value) => new IsSterilized(value);
    }
}
