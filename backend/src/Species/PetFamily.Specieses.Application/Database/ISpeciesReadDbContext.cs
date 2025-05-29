using Microsoft.EntityFrameworkCore;
using PetFamily.Core.DTOs;
using PetFamily.Specieses.Contracts.DTOs;

namespace PetFamily.Specieses.Application.Database
{
    public interface ISpeciesReadDbContext
    {
        IQueryable<SpeciesDto> Species { get; }
        IQueryable<BreedDto> Breeds { get; }
    }
}
