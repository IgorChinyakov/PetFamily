using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Application.Features.Queries.GetSubmittedWithPagination;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Queries.GetRequestsByAdminId
{
    public class GetRequestsByAdminIdHandler : 
        IQueryHandler<PagedList<VolunteerRequestDto>, GetRequestsByAdminIdQuery>
    {
        private readonly IVolunteerRequestsReadDbContext _readDbContext;

        public GetRequestsByAdminIdHandler(
            IVolunteerRequestsReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<VolunteerRequestDto>> Handle(
            GetRequestsByAdminIdQuery query, 
            CancellationToken cancellationToken = default)
        {
            var requestsQuery = _readDbContext.RequestDtos;

            requestsQuery = requestsQuery.Where(
                r => r.AdminId == query.AdminId);

            requestsQuery = requestsQuery.WhereIf(
                query.Status != null,
                r => r.Status == query.Status,
                cancellationToken);

            return await requestsQuery
                .GetWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
