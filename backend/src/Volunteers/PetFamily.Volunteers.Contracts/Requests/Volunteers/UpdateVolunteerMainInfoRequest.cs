using PetFamily.Core.DTOs;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteers
{
    public record UpdateVolunteerMainInfoRequest(
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber);
}
