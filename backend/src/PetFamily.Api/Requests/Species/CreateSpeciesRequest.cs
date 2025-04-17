using PetFamily.Application.EntitiesHandling.Specieses.Commands.Create;

namespace PetFamily.Api.Requests.Species
{
    public record CreateSpeciesRequest(string Name)
    {
        public CreateSpeciesCommand ToCommand()
            => new CreateSpeciesCommand(Name);
    }
}