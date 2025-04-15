using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination
{
    public class GetVolunteersWithPaginationHandler 
        : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetVolunteersWithPaginationHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<VolunteerDto>> Handle(
            GetVolunteersWithPaginationQuery query, 
            CancellationToken cancellationToken = default)
        {
            var volunteersQuery = _readDbContext.Volunteers;

            var pagedList = await volunteersQuery
                .GetWithPagination<VolunteerDto>(query.Page, query.PageSize, cancellationToken);

            return pagedList;
        }
    }
}
