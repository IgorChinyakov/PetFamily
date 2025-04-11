using PetFamily.Application.Volunteers.UseCases.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.UseCases.UpdateDetails
{
    public record UpdateDetailsCommand(
        Guid VolunteerId, 
        IEnumerable<DetailsDto> Details);
}
