using PetFamily.Application.EntitiesHandling.Breeds.Commands.Create;
using System.Globalization;

namespace PetFamily.Api.Requests.Breeds
{
    public record CreateBreedRequest(string Name)
    {
        public CreateBreedCommand ToCommand(Guid speciesId)
            => new CreateBreedCommand(speciesId, Name);
    }
}