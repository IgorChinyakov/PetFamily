using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Application.Volunteers.UpdateDetails;
using PetFamily.Domain.VolunteerContext.SharedVO;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateDetailsRequest(IEnumerable<DetailsDto> Details)
    {
        public UpdateDetailsCommand ToCommand(Guid volunteerId)
            => new UpdateDetailsCommand(volunteerId, Details);
    }
}
