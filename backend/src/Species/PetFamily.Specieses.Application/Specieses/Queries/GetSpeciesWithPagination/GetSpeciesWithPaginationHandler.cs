using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using PetFamily.Core.Extensions;
using PetFamily.Specieses.Contracts.DTOs;

namespace PetFamily.Specieses.Application.Specieses.Queries.GetSpeciesWithPagination
{
    public class GetSpeciesWithPaginationHandler :
        IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery>
    {
        private readonly ISpeciesReadDbContext _readDbContext;

        public GetSpeciesWithPaginationHandler(ISpeciesReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<SpeciesDto>> Handle(
            GetSpeciesWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var speciesQuery = _readDbContext.Species;

            var pagedList = await speciesQuery
                .GetWithPagination(query.Page, query.PageSize, cancellationToken);

            return pagedList;
        }
    }
}
