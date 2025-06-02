using PetFamily.Core.DTOs;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteers
{
    public record UpdateVolunteerDetailsRequest(IEnumerable<DetailsDto> Details);
}
