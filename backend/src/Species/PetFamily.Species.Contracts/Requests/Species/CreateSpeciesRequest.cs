using PetFamily.Specieses.Application.Specieses.Commands.Create;

namespace PetFamily.Specieses.Contracts.Requests.Species
{
    public record CreateSpeciesRequest(string Name)
    {
        public CreateSpeciesCommand ToCommand()
            => new CreateSpeciesCommand(Name);
    }
}