using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Commands.Update
{
    public record UpdateRequestCommand(Guid RequestId, Guid UserId, string UpdatedInformation) : ICommand;
}
