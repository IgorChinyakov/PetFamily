using PetFamily.Application.Volunteers.UseCases.DTOs;
using PetFamily.Application.Volunteers.UseCases.UpdateSocialMedia;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateSocialMediaRequest(IEnumerable<SocialMediaDto> SocialMedia)
    {
        public UpdateSocialMediaCommand ToCommand(Guid volunteerId)
            => new UpdateSocialMediaCommand(volunteerId, SocialMedia);
    }
}
