using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record CreationDate
    {
        public DateTime Value { get; }

        private CreationDate(DateTime value)
        {
            Value = value;
        }

        public static Result<CreationDate> Create(DateTime value) => new CreationDate(value);
    }
}
