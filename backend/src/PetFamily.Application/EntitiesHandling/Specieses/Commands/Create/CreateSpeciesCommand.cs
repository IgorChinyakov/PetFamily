using PetFamily.Application.Abstractions;

namespace PetFamily.Application.EntitiesHandling.Specieses.Commands.Create
{
    public record CreateSpeciesCommand(string Name) : ICommand;
}