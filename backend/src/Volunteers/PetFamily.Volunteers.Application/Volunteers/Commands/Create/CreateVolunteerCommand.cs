using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Create
{
    public record CreateVolunteerCommand(
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber,
        List<DetailsDto> DetailsList,
        List<SocialMediaDto> SocialMediaList) : ICommand;
}
