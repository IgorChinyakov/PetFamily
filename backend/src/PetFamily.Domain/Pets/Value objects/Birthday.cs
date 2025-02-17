using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record Birthday
    {
        public DateTime Value { get; }

        private Birthday(DateTime value)
        {
            Value = value;
        }

        public static Result<Birthday> Create(DateTime value) => new Birthday(value);
    }
}
