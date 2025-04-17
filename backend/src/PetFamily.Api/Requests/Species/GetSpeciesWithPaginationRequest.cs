using PetFamily.Application.EntitiesHandling.Specieses.Queries.GetSpeciesWithPagination;
using PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetFamily.Api.Requests.Species
{
    public record GetSpeciesWithPaginationRequest(int Page, int PageSize)
    {
        public GetSpeciesWithPaginationQuery ToQuery()
            => new GetSpeciesWithPaginationQuery(Page, PageSize);
    }
}
