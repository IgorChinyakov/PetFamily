using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Contracts.Messaging
{
    public record RequestTakenOnReviewEvent(Guid RequestId, IEnumerable<Guid> UserIds);
}
