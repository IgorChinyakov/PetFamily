using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Pets.Commands.Move
{
    public record MovePetCommand(Guid VolunteerId, Guid PetId, int PositionToMove) : ICommand;
}
