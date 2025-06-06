using PetFamily.Core.DTOs;

namespace PetFamily.Accounts.Contracts.DTOs
{
    public record VolunteerAccountDto(
        Guid Id,
        Guid Userid,
        int Experience,
        List<DetailsDto> Details);
}