using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Contracts.DTOs
{
    public record VolunteerRequestDto(
        Guid Id,
        Guid UserId,
        Guid? AdminId, 
        Guid? DiscussionId, 
        DateTime CreationDate, 
        RequestStatusDto Status,
        string VolunteerInformation,
        string? RejectionComment);
}
