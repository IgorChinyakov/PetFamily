using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record SpeciesId
    {
        private SpeciesId(Guid id)
        {
            Value = id;
        }

        public Guid Value { get; }

        public static Result<SpeciesId> Create(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Failure<SpeciesId>("Guid is not supposed to be empty");

            return new SpeciesId(id);
        }
    }
}
