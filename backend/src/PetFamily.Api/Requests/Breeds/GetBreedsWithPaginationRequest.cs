using PetFamily.Application.EntitiesHandling.Breeds.Queries.GetBreedsWithPagination;

namespace PetFamily.Api.Requests.Breeds
{
    public record GetBreedsWithPaginationRequest(int Page, int PageSize)
    {
        public GetBreedsWithPaginationQuery ToQuery(Guid SpeciesId)
            => new GetBreedsWithPaginationQuery(SpeciesId, Page, PageSize);
    }
}
