using PetFamily.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.EntitiesHandling.Breeds.Commands.Delete
{
    public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
}
