using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Domain.ValueObjects.Discussion
{
    public record DiscussionId
    {
        private DiscussionId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static DiscussionId Create(Guid value) => new(value);

        public static DiscussionId New() => new(Guid.NewGuid());

        public static DiscussionId Empty() => new(Guid.Empty);
    }
}
