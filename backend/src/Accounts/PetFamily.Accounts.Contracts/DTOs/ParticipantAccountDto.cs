using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Contracts.DTOs
{
    public record ParticipantAccountDto(
        Guid Id,
        Guid UserId,
        List<Guid> FavoritePets);
}
