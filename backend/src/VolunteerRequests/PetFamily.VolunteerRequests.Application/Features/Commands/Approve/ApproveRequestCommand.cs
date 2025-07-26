using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Commands.Approve
{
    public record ApproveRequestCommand(Guid RequestId, Guid AdminId) : ICommand;
}
