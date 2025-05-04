using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateDetails;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteers
{
    public record UpdateVolunteerDetailsRequest(IEnumerable<DetailsDto> Details)
    {
        public UpdateVolunteerDetailsCommand ToCommand(Guid volunteerId)
            => new UpdateVolunteerDetailsCommand(volunteerId, Details);
    }
}
