using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Domain.ValueObjects.Discussion
{
    public record RelationId
    {
        private RelationId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static RelationId Create(Guid value) => new(value);

        public static RelationId New() => new(Guid.NewGuid());

        public static RelationId Empty() => new(Guid.Empty);
    }
}
