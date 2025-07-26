using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Queries.GetRequestsByAdminId
{
    public record GetRequestsByAdminIdQuery(Guid AdminId, int Page, int PageSize, RequestStatusDto? Status = null) : IQuery;
}
