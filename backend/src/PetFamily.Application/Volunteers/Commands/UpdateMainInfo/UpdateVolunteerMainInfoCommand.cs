using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfo
{
    public record UpdateVolunteerMainInfoCommand(
        Guid VolunteerId, 
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber) : ICommand;
}
