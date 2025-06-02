using PetFamily.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Contracts.Requests
{
    public record UpdateVolunteerDetailsRequest(IEnumerable<DetailsDto> Details);
}
