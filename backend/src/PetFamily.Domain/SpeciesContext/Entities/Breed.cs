using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesContext.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.SpeciesContext.Entities
{
    public class Breed : SoftDeletableEntity
    {
        public Name Name { get; private set; }

        private Breed(Guid id) : base(id)
        {
        }

        public Breed(Guid id, Name name) : base(id)
        {
            Name = name;
        }
    }
}
