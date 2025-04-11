using PetFamily.Application.Volunteers.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.UseCases.UpdateMainInfo
{
    public record UpdateMainInfoCommand(
        Guid VolunteerId, 
        FullNameDto FullName,
        string Email,
        string Description,
        int Experience,
        string PhoneNumber);
}
