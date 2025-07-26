using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Queries.GetSubmittedWithPagination
{
    public class GetSubmittedRequestsHandler : 
        IQueryHandler<PagedList<VolunteerRequestDto>, GetSubmittedRequestsQuery>
    {
        private readonly IVolunteerRequestsReadDbContext _readDbContext;

        public GetSubmittedRequestsHandler(
            IVolunteerRequestsReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<VolunteerRequestDto>> Handle(
            GetSubmittedRequestsQuery query, 
            CancellationToken cancellationToken = default)
        {
            var requestsQuery = _readDbContext.RequestDtos;

            requestsQuery = requestsQuery.Where(r => r.Status == RequestStatusDto.Submitted);

            return await requestsQuery
                .GetWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
