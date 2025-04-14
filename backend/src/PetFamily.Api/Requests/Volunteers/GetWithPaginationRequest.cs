using PetFamily.Application.Volunteers.Commands.UpdateDetails;
using PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Domain.VolunteerContext.SharedVO;

namespace PetFamily.Api.Requests.Volunteers
{
    public record GetWithPaginationRequest(int Page, int PageSize)
    {
        public GetVolunteersWithPaginationQuery ToQuery()
            => new GetVolunteersWithPaginationQuery(Page, PageSize);
    }
}
