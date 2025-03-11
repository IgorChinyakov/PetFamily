using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateSocialMediaRequest(IEnumerable<SocialMediaDto> SocialMedia);
}
