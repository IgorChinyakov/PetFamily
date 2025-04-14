using PetFamily.Application.Volunteers.Commands.UpdateDetails;
using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateDetailsRequest(IEnumerable<DetailsDto> Details)
    {
        public UpdateDetailsCommand ToCommand(Guid volunteerId)
            => new UpdateDetailsCommand(volunteerId, Details);
    }
}
