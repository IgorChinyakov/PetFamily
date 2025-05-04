using PetFamily.Core.Abstractions;

namespace PetFamily.Specieses.Application.Breeds.Queries.GetBreedsWithPagination
{
    public record GetBreedsWithPaginationQuery(
        Guid SpeciesId, int Page, int PageSize) : IQuery;
}
