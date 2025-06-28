using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Domain.ValueObjects.Shared
{
    public record UserId
    {
        private UserId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static UserId Create(Guid value) => new(value);

        public static UserId New() => new(Guid.NewGuid());

        public static UserId Empty() => new(Guid.Empty);
    }
}
