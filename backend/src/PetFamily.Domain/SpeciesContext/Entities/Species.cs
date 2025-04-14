using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesContext.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.SpeciesContext.Entities
{
    public class Species : SoftDeletableEntity
    {
        private readonly List<Breed> _breeds = [];

        public IReadOnlyList<Breed> Breeds => _breeds;
        public Name Name { get; set; }

        private Species(Guid id) : base(id)
        {
        }

        public Species(Name name)
        {
            Name = name;
        }
    }
}
