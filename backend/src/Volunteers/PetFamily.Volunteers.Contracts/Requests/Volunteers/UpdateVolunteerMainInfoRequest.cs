using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteers
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
