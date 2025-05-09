using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteers
{
    public record UpdateVolunteerSocialMediaRequest(IEnumerable<SocialMediaDto> SocialMedia);
}
