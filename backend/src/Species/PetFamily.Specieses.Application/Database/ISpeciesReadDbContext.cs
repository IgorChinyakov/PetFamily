using Microsoft.EntityFrameworkCore;
using PetFamily.Core.DTOs;
using PetFamily.Specieses.Contracts.DTOs;

namespace PetFamily.Core.Abstractions.Database
{
    public interface ISpeciesReadDbContext
    {
        IQueryable<SpeciesDto> Species { get; }
        IQueryable<BreedDto> Breeds { get; }
    }
}
