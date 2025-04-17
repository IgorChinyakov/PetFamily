using PetFamily.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.EntitiesHandling.Specieses.Commands.Delete
{
    public record DeleteSpeciesCommand(Guid Id) : ICommand;
}
