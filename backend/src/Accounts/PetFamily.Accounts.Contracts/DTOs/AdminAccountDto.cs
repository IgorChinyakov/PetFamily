using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Contracts.DTOs
{
    public record AdminAccountDto(
        Guid Id,
        Guid UserId);
}
