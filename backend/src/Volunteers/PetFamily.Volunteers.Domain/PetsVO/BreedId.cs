using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Domain.PetsVO
{
    public record BreedId
    {
        public Guid Value { get; }

        private BreedId(Guid id)
        {
            Value = id;
        }

        public static Result<BreedId, Error> Create(Guid id)
        {
            if (id == Guid.Empty)
                return Errors.General.ValueIsInvalid("Guid");

            return new BreedId(id);
        }
    }
}
