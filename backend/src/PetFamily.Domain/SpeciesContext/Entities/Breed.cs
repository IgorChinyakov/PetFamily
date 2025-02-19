using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesContext.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.SpeciesContext.Entities
{
    public class Breed : Entity<Guid>
    {
        public Name Name { get; set; }

        private Breed(Guid id) : base(id)
        {

        }

        private Breed(Guid id, Name name) : base(id)
        {
            Name = name;
        }
    }
}
