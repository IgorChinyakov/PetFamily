using PetFamily.Core.Abstractions;

namespace PetFamily.Specieses.Application.Specieses.Commands.Delete
{
    public record DeleteSpeciesCommand(Guid Id) : ICommand;
}
