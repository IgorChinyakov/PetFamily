using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Domain.Entities;

namespace PetFamily.Specieses.Application.Database
{
    public interface ISpeciesRepository
    {
        Task<Guid> Add(Species species, CancellationToken token = default);

        Task<Result<Breed, Error>> GetBreedById(Guid speciesId, Guid breedId, CancellationToken token = default);

        Task<Result<Species, Error>> GetById(Guid speciesId, CancellationToken token = default);

        Guid HardDelete(Species species, CancellationToken token = default);

        Guid SoftDelete(Species species, CancellationToken token = default);
    }
}
