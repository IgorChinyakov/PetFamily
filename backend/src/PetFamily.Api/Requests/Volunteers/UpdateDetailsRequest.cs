using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Domain.VolunteerContext.SharedVO;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateDetailsRequest(IEnumerable<DetailsDto> Details);
}
