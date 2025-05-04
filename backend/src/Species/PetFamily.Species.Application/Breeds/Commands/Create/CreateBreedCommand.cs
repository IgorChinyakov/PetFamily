using PetFamily.Core.Abstractions;

namespace PetFamily.Specieses.Application.Breeds.Commands.Create
{
    public record CreateBreedCommand(Guid SpeciesId, string Name) : ICommand;
}