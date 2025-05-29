using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteers
{
    public record CreateVolunteerRequest(
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber);
}
