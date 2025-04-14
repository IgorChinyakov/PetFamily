using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Specieses;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly WriteDbContext _context;

        public SpeciesRepository(WriteDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Species, Error>> GetById(
            Guid speciesId, Guid breedId, CancellationToken token = default)
        {
            var species = await _context
                .Species
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => s.Id == speciesId);

            if (species == null)
                return Errors.General.NotFound(speciesId);

            var breed = species
                .Breeds
                .FirstOrDefault(b => b.Id == breedId);

            if (breed == null)
                return Errors.General.NotFound(breedId);

            return species;
        }
    }
}
