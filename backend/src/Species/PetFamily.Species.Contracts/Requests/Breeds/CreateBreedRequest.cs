using PetFamily.Specieses.Application.Breeds.Commands.Create;
using System.Globalization;

namespace PetFamily.Specieses.Contracts.Requests.Breeds
{
    public record CreateBreedRequest(string Name)
    {
        public CreateBreedCommand ToCommand(Guid speciesId)
            => new CreateBreedCommand(speciesId, Name);
    }
}