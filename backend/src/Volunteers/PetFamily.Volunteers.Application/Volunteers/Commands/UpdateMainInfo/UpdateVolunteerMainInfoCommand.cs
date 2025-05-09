using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo
{
    public record UpdateVolunteerMainInfoCommand(
        Guid VolunteerId,
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber) : ICommand;
}
