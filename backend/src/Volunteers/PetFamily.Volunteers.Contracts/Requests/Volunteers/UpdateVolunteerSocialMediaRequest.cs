using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialMedia;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteers
{
    public record UpdateVolunteerSocialMediaRequest(IEnumerable<SocialMediaDto> SocialMedia)
    {
        public UpdateVolunteerSocialMediaCommand ToCommand(Guid volunteerId)
            => new UpdateVolunteerSocialMediaCommand(volunteerId, SocialMedia);
    }
}
