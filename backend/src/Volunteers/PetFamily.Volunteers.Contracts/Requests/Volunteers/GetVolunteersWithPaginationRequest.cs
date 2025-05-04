using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteers
{
    public record GetVolunteersWithPaginationRequest(int Page, int PageSize)
    {
        public GetVolunteersWithPaginationQuery ToQuery()
            => new GetVolunteersWithPaginationQuery(Page, PageSize);
    }
}
