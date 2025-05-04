using PetFamily.Core.Abstractions;

namespace PetFamily.Specieses.Application.Specieses.Commands.Create
{
    public record CreateSpeciesCommand(string Name) : ICommand;
}