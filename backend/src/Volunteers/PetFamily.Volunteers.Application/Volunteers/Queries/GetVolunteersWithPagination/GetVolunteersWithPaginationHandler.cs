using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.DTOs;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination
{
    public class GetVolunteersWithPaginationHandler
        : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
    {
        private readonly IVolunteersReadDbContext _readDbContext;

        public GetVolunteersWithPaginationHandler(IVolunteersReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<VolunteerDto>> Handle(
            GetVolunteersWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var volunteersQuery = _readDbContext.Volunteers;

            var pagedList = await volunteersQuery
                .GetWithPagination(query.Page, query.PageSize, cancellationToken);

            return pagedList;
        }
    }
}
