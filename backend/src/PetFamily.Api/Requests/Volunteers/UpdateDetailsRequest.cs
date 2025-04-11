using PetFamily.Application.Volunteers.UseCases.DTOs;
using PetFamily.Application.Volunteers.UseCases.UpdateDetails;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateDetailsRequest(IEnumerable<DetailsDto> Details)
    {
        public UpdateDetailsCommand ToCommand(Guid volunteerId)
            => new UpdateDetailsCommand(volunteerId, Details);
    }
}
