using PetFamily.Application.DTOs;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateDetails;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateVolunteerDetailsRequest(IEnumerable<DetailsDto> Details)
    {
        public UpdateVolunteerDetailsCommand ToCommand(Guid volunteerId)
            => new UpdateVolunteerDetailsCommand(volunteerId, Details);
    }
}
