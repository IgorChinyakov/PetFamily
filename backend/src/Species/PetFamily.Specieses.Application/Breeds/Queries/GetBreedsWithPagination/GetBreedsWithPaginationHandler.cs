using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.DTOs;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Specieses.Application.Breeds.Queries.GetBreedsWithPagination
{
    public class GetBreedsWithPaginationHandler :
        IQueryHandler<PagedList<BreedDto>, GetBreedsWithPaginationQuery>
    {
        private readonly ISpeciesReadDbContext _readDbContext;

        public GetBreedsWithPaginationHandler(ISpeciesReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<BreedDto>> Handle(
            GetBreedsWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var breeds = await _readDbContext.Breeds
                .Where(b => b.SpeciesId == query.SpeciesId)
                .GetWithPagination(query.Page, query.PageSize);

            return breeds;
        }
    }
}
