using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesContext.Entities;

namespace PetFamily.Application.Specieses
{
    public interface ISpeciesRepository
    {
        Task<Result<Species, Error>> GetById(Guid speciesId, Guid breedId, CancellationToken token = default);
    }
}
