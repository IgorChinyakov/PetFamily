using CSharpFunctionalExtensions;
using PetFamily.Core.DTOs;
using PetFamily.SharedKernel;

namespace PetFamily.Specieses.Contracts
{
    public interface ISpeciesContract
    {
        Task<Result<SpeciesDto, ErrorsList>> GetSpeciesById(
            Guid id, CancellationToken cancellationToken = default);

        Task<Result<BreedDto, ErrorsList>> GetBreedById(
            Guid speciesId, Guid breedId, CancellationToken cancellationToekn = default);
    }
}
