using PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Specieses.Application.Specieses.Queries.GetSpeciesWithPagination;

namespace PetFamily.Specieses.Contracts.Requests.Species
{
    public record GetSpeciesWithPaginationRequest(int Page, int PageSize)
    {
        public GetSpeciesWithPaginationQuery ToQuery()
            => new GetSpeciesWithPaginationQuery(Page, PageSize);
    }
}
