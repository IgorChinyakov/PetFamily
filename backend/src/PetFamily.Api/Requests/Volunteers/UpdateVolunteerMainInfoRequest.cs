using PetFamily.Application.DTOs;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateMainInfo;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateVolunteerMainInfoRequest(
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber)
    {
        public UpdateVolunteerMainInfoCommand ToCommand(Guid volunteerId)
            => new UpdateVolunteerMainInfoCommand(
                volunteerId, 
                FullName, 
                Email, 
                Description, 
                Experience, 
                PhoneNumber);
    }
}
