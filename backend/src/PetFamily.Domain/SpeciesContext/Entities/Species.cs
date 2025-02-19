using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesContext.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.SpeciesContext.Entities
{
    public class Species : Entity<Guid>
    {
        private readonly IReadOnlyList<Breed> _breeds = [];

        public Name Name { get; set; }

        private Species(Guid id) : base(id)
        {

        }

        private Species(Guid id, Name name) : base(id)
        {
            Name = name;
        }
    }
}
