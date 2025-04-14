using PetFamily.Application.Volunteers.Commands.UpdateSocialMedia;
using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateSocialMediaRequest(IEnumerable<SocialMediaDto> SocialMedia)
    {
        public UpdateSocialMediaCommand ToCommand(Guid volunteerId)
            => new UpdateSocialMediaCommand(volunteerId, SocialMedia);
    }
}
