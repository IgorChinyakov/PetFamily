using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Application.Volunteers.UpdateDetails;
using PetFamily.Application.Volunteers.UpdateSocialMedia;
using PetFamily.Domain.VolunteerContext.SharedVO;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateSocialMediaRequest(IEnumerable<SocialMediaDto> SocialMedia)
    {
        public UpdateSocialMediaCommand ToCommand(Guid volunteerId)
            => new UpdateSocialMediaCommand(volunteerId, SocialMedia);
    }
}
