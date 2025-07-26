using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Queries.GetSubmittedWithPagination
{
    public record GetSubmittedRequestsQuery(int Page, int PageSize) : IQuery;
}
