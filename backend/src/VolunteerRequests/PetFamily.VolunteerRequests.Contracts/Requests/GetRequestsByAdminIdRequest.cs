using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Contracts.Requests
{
    public record GetRequestsByAdminIdRequest(int Page, int PageSize, RequestStatusDto? Status = null);
}
