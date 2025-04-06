using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.Api.Requests.Volunteers
{
    public record UpdateMainInfoRequest(
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber)
    {
        public UpdateMainInfoCommand ToCommand(Guid volunteerId)
            => new UpdateMainInfoCommand(
                volunteerId, 
                FullName, 
                Email, 
                Description, 
                Experience, 
                PhoneNumber);
    }
}
