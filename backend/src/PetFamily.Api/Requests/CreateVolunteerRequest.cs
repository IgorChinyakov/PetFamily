using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Api.Contracts
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
