using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Application.Features.Commands.UpdateDiscussionId;
using PetFamily.VolunteerRequests.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Presentation
{
    public class VolunteerRequestsContract : IVolunteerRequestsContract
    {
        private readonly UpdateDiscussionIdHandler _updateDiscussionIdHandler;

        public VolunteerRequestsContract(UpdateDiscussionIdHandler updateDiscussionIdHandler)
        {
            _updateDiscussionIdHandler = updateDiscussionIdHandler;
        }

        public async Task<UnitResult<ErrorsList>> UpdateDiscussionId(Guid RequestId, Guid DiscussionId)
        => await _updateDiscussionIdHandler.Handle(new UpdateDiscussionIdCommand(RequestId, DiscussionId));
    }
}
