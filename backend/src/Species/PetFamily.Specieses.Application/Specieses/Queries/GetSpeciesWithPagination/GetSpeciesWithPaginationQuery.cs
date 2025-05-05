using PetFamily.Core.Abstractions;

namespace PetFamily.Specieses.Application.Specieses.Queries.GetSpeciesWithPagination
{
    public record GetSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery
    {
        public GetSpeciesWithPaginationQuery ToQuery()
            => new GetSpeciesWithPaginationQuery(Page, PageSize);
    }
}
