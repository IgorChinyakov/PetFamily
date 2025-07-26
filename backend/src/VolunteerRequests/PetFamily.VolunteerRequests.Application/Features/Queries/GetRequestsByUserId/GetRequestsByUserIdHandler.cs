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

namespace PetFamily.VolunteerRequests.Application.Features.Queries.GetRequestsByuserId
{
    public class GetRequestsByUserIdHandler : 
        IQueryHandler<PagedList<VolunteerRequestDto>, GetRequestsByUserIdQuery>
    {
        private readonly IVolunteerRequestsReadDbContext _readDbContext;

        public GetRequestsByUserIdHandler(
            IVolunteerRequestsReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<VolunteerRequestDto>> Handle(
            GetRequestsByUserIdQuery query, 
            CancellationToken cancellationToken = default)
        {
            var requestsQuery = _readDbContext.RequestDtos;

            requestsQuery = requestsQuery.Where(
                r => r.UserId == query.UserId);

            requestsQuery = requestsQuery.WhereIf(
                query.Status != null,
                r => r.Status == query.Status,
                cancellationToken);

            return await requestsQuery
                .GetWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
