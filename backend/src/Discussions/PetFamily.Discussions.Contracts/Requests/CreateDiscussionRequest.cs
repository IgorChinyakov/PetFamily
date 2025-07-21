using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Contracts.Requests
{
    public record CreateDiscussionRequest(Guid RelationId, IEnumerable<Guid> UserIds);
}
