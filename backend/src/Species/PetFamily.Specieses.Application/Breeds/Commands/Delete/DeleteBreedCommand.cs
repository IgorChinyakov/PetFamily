using PetFamily.Core.Abstractions;

namespace PetFamily.Specieses.Application.Breeds.Commands.Delete
{
    public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
}
