using AutoFixture;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Create;
using PetFamily.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests
{
    public class TestsBase :
        IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
    {
        protected readonly WriteDbContext WriteDbContext;
        protected readonly IReadDbContext ReadDbContext;
        protected readonly IServiceScope Scope;
        protected readonly IFixture Fixture;
        protected readonly IntegrationTestsWebFactory Factory;

        public TestsBase(
            IntegrationTestsWebFactory factory)
        {
            Factory = factory;
            Fixture = new Fixture();
            Scope = factory.Services.CreateScope();
            ReadDbContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
            WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            Scope.Dispose();
            await Factory.ResetDatabase();
        }

        public async Task<Guid> SeedSpecies()
        {
            var species = Fixture.CreateSpecies();

            await WriteDbContext.Species.AddAsync(species);

            await WriteDbContext.SaveChangesAsync();

            return species.Id;
        }

        public async Task<Guid> SeedBreed(Guid speciesId)
        {
            var breed = Fixture.CreateBreed();

            var species = await WriteDbContext.Species.FirstOrDefaultAsync(s => s.Id == speciesId);

            species!.AddBreed(breed);

            await WriteDbContext.SaveChangesAsync();

            return breed.Id;
        }

        public async Task<Guid> SeedPet(Guid speciesId, Guid breedId, Guid volunteerId)
        {
            var pet = Fixture.CreatePet(speciesId, breedId);

            var volunteer = await WriteDbContext.Volunteers
                .FirstOrDefaultAsync(s => s.Id == volunteerId);

            volunteer!.AddPet(pet);

            await WriteDbContext.SaveChangesAsync();

            return pet.Id;
        }

        public async Task<Guid> SeedVolunteer()
        {
            var volunteer = Fixture.CreateVolunteer();

            await WriteDbContext.Volunteers.AddAsync(volunteer);

            await WriteDbContext.SaveChangesAsync();

            return volunteer.Id;
        }

        public async Task SeedVolunteers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var volunteer = Fixture.CreateVolunteer();

                await WriteDbContext.Volunteers.AddAsync(volunteer);
            }

            await WriteDbContext.SaveChangesAsync();
        }

        public async Task SeedSpecieses(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var species = Fixture.CreateSpecies();

                await WriteDbContext.Species.AddAsync(species);
            }

            await WriteDbContext.SaveChangesAsync();
        }

        public async Task SeedPets(Guid speciesId, Guid breedId, Guid volunteerId, int count)
        {
            var volunteer = await WriteDbContext.Volunteers
                .FirstOrDefaultAsync(s => s.Id == volunteerId);

            for (int i = 0; i < count; i++)
            {
                var pet = Fixture.CreatePet(speciesId, breedId);

                volunteer!.AddPet(pet);
            }

            await WriteDbContext.SaveChangesAsync();
        }

        public async Task SeedBreeds(Guid speciesId, int count)
        {
            var species = await WriteDbContext.Species
                .FirstOrDefaultAsync(s => s.Id == speciesId);

            for (int i = 0; i < count; i++)
            {
                var breed = Fixture.CreateBreed();

                species!.AddBreed(breed);
            }

            await WriteDbContext.SaveChangesAsync();
        }
    }
}
