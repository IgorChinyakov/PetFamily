using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateMainInfoRequest(
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber);
}
