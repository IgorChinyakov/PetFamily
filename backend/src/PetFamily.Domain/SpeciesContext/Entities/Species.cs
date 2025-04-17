using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesContext.ValueObjects;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.PetsVO;
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
        public Name Name { get; private set; }

        private Species(Guid id) : base(id)
        {
        }

        public Species(Guid id, Name name) : base(id)
        {
            Name = name;
        }

        public UnitResult<Error> AddBreed(Breed breed)
        {
            _breeds.Add(breed);

            return Result.Success<Error>();
        }

        public UnitResult<Error> RemoveBreed(Breed breed)
        {
            _breeds.Remove(breed);

            return Result.Success<Error>();
        }

        public override void Delete()
        {
            base.Delete();

            foreach (var breed in _breeds)
            {
                breed.Delete();
            }
        }
    }
}
