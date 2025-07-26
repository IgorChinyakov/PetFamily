using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Commands.Reject
{
    public record RejectRequestCommand(Guid AdminId, Guid RequestId) : ICommand;
}
