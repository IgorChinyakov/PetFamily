using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record BreedId
    {
        public Guid Id { get; }

        private BreedId(Guid id)
        {
            Id = id;
        }

        public static Result<BreedId> Create(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Failure<BreedId>("Guid is not supposed to be empty");

            return new BreedId(id);
        }
    }
}
