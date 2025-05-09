using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application.Database;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly SpeciesWriteDbContext _context;

        public SpeciesRepository(SpeciesWriteDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Species, Error>> GetById(
            Guid speciesId, CancellationToken token = default)
        {
            var species = await _context
                .Species
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => s.Id == speciesId);

            if (species == null)
                return Errors.General.NotFound(speciesId);

            return species;
        }

        public async Task<Result<Breed, Error>> GetBreedById(
            Guid speciesId, Guid breedId, CancellationToken token = default)
        {
            var species = await GetById(speciesId, token);
            if (species.IsFailure)
                return Errors.General.NotFound(speciesId);

            var breed = species.Value.Breeds.FirstOrDefault(b => b.Id == breedId);
            if(breed == null)
                return Errors.General.NotFound(breedId);

            return breed;
        }

        public async Task<Guid> Add(
            Species species, CancellationToken token = default)
        {
            await _context.Species.AddAsync(species, token);

            return species.Id;
        }

        public Guid SoftDelete(
            Species species, CancellationToken token = default)
        {
            species.Delete();

            return species.Id;
        }

        public Guid HardDelete(
            Species species, CancellationToken token = default)
        {
            _context.Remove(species);

            return species.Id;
        }
    }
}
