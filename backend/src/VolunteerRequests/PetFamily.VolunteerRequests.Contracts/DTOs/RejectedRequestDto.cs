using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Contracts.DTOs
{
    public record RejectedRequestDto(Guid RequestId, DateTime RejectionDate);
}
