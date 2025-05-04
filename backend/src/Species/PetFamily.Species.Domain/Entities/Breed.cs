using PetFamily.SharedKernel;
using PetFamily.Specieses.Domain.ValueObjects;

namespace PetFamily.Specieses.Domain.Entities
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
