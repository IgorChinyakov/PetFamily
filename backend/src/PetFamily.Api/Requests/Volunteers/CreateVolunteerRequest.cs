using PetFamily.Application.Volunteers.UseCases.Create;
using PetFamily.Application.Volunteers.UseCases.DTOs;

namespace PetFamily.Api.Requests.Volunteers
{
    public record CreateVolunteerRequest(
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber,
        List<DetailsDto> DetailsList,
        List<SocialMediaDto> SocialMediaList)
    {
        public CreateVolunteerCommand ToCommand()
            => new CreateVolunteerCommand(
                FullName, 
                Email, 
                Description, 
                Experience, 
                PhoneNumber, 
                DetailsList, 
                SocialMediaList);
    }
}
