using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record PetId
    {
        public Guid Id { get; }

        private PetId(Guid id)
        {
            Id = id;
        }

        public static PetId CreateNew() => new PetId(Guid.NewGuid());
        public static PetId CreateEmpty() => new PetId(Guid.Empty);
        public static PetId Create(Guid id) => new PetId(id);
    }
}
