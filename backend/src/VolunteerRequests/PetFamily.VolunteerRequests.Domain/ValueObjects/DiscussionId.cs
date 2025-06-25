using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects
{
    public record DiscussionId
    {
        public Guid Value { get; }

        //ef core
        private DiscussionId()
        {
        }

        private DiscussionId(Guid id)
        {
            Value = id;
        }

        public static DiscussionId NewDiscussionId() => new(Guid.NewGuid());

        public static DiscussionId Empty() => new(Guid.Empty);

        public static DiscussionId Create(Guid id) => new(id);
    }
}
