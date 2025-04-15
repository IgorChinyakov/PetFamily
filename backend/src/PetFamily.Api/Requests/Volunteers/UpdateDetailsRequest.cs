using PetFamily.Application.DTOs;
using PetFamily.Application.Volunteers.Commands.UpdateDetails;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateDetailsRequest(IEnumerable<DetailsDto> Details)
    {
        public UpdateDetailsCommand ToCommand(Guid volunteerId)
            => new UpdateDetailsCommand(volunteerId, Details);
    }
}
