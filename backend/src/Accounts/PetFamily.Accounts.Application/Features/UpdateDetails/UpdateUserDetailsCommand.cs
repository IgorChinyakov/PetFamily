using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Features.UpdateDetails
{
    public record UpdateUserDetailsCommand(
        Guid Id,
        IEnumerable<DetailsDto> Details) : ICommand;
}
