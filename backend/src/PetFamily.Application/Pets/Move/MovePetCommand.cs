using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Pets.Move
{
    public record MovePetCommand(Guid VolunteerId, Guid PetId, int PositionToMove);
}
