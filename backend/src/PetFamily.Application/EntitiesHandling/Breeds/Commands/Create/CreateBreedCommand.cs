using PetFamily.Application.Abstractions;

namespace PetFamily.Application.EntitiesHandling.Breeds.Commands.Create
{
    public record CreateBreedCommand(Guid SpeciesId, string Name) : ICommand;
}