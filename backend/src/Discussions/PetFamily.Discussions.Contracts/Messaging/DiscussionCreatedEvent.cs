using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Contracts.Messaging
{
    public record DiscussionCreatedEvent(Guid RelationId, Guid DiscussionId);
}
