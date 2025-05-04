using PetFamily.Specieses.Application.Breeds.Queries.GetBreedsWithPagination;

namespace PetFamily.Specieses.Contracts.Requests.Breeds
{
    public record GetBreedsWithPaginationRequest(int Page, int PageSize)
    {
        public GetBreedsWithPaginationQuery ToQuery(Guid SpeciesId)
            => new GetBreedsWithPaginationQuery(SpeciesId, Page, PageSize);
    }
}
