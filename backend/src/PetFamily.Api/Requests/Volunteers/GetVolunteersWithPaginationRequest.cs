using PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetFamily.Api.Requests.Volunteers
{
    public record GetVolunteersWithPaginationRequest(int Page, int PageSize)
    {
        public GetVolunteersWithPaginationQuery ToQuery()
            => new GetVolunteersWithPaginationQuery(Page, PageSize);
    }
}
