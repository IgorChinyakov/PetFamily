using PetFamily.Application.DTOs;
using PetFamily.Application.Volunteers.Commands.UpdateSocialMedia;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateSocialMediaRequest(IEnumerable<SocialMediaDto> SocialMedia)
    {
        public UpdateSocialMediaCommand ToCommand(Guid volunteerId)
            => new UpdateSocialMediaCommand(volunteerId, SocialMedia);
    }
}
