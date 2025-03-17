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
        public Name Name { get; set; }

        private Breed(Guid id) : base(id)
        {
        }

        public Breed(Name name) : base()
        {
            Name = name;
        }
    }
}
