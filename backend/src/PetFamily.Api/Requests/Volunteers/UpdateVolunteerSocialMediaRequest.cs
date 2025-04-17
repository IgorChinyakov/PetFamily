using PetFamily.Application.DTOs;
using PetFamily.Application.Volunteers.Commands.UpdateSocialMedia;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateVolunteerSocialMediaRequest(IEnumerable<SocialMediaDto> SocialMedia)
    {
        public UpdateVolunteerSocialMediaCommand ToCommand(Guid volunteerId)
            => new UpdateVolunteerSocialMediaCommand(volunteerId, SocialMedia);
    }
}
