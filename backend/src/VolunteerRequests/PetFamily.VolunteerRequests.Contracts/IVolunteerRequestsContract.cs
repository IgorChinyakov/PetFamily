using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Contracts
{
    public interface IVolunteerRequestsContract
    {
        Task<UnitResult<ErrorsList>> UpdateDiscussionId(Guid RequestId, Guid DiscussionId);
    }
}
