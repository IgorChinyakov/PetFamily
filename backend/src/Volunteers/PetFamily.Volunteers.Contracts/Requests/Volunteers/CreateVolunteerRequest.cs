using PetFamily.Core.DTOs;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteers
{
    public record CreateVolunteerRequest(
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber,
        List<DetailsDto> DetailsList,
        List<SocialMediaDto> SocialMediaList);
}
