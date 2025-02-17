using CSharpFunctionalExtensions;
using PetFamily.Domain.Species.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Species
{
    public class Species : Entity<Guid>
    {
        private readonly List<Breed> _breeds = [];

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
