using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Infrastructure.Authorization.Managers;
using PetFamily.Specieses.Application.Database;
using PetFamily.Specieses.Infrastructure.DbContexts;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests
{
    public class TestsBase :
        IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
    {
        protected readonly SpeciesWriteDbContext SpeciesWriteDbContext;
        protected readonly VolunteersWriteDbContext VolunteersWriteDbContext;
        protected readonly ISpeciesReadDbContext SpeciesReadDbContext;
        protected readonly IVolunteersReadDbContext VolunteersReadDbContext;
        protected readonly UserManager<User> UserManager;
        protected readonly RoleManager<Role> RoleManager;
        protected readonly IParticipantAccountManager ParticipantAccountManager;
        protected readonly IVolunteerAccountManager VolunteerAccountManager;
        protected readonly IServiceScope Scope;
        protected readonly IFixture Fixture;
        protected readonly IntegrationTestsWebFactory Factory;

        public TestsBase(
            IntegrationTestsWebFactory factory)
        {
            Factory = factory;
            Fixture = new Fixture();
            Scope = factory.Services.CreateScope();
            SpeciesReadDbContext = Scope.ServiceProvider.GetRequiredService<ISpeciesReadDbContext>();
            VolunteersReadDbContext = Scope.ServiceProvider.GetRequiredService<IVolunteersReadDbContext>();
            SpeciesWriteDbContext = Scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
            VolunteersWriteDbContext = Scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
            UserManager = Scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            RoleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            ParticipantAccountManager = Scope.ServiceProvider.GetRequiredService<IParticipantAccountManager>();
            VolunteerAccountManager = Scope.ServiceProvider.GetRequiredService<IVolunteerAccountManager>();
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

            await SpeciesWriteDbContext.Species.AddAsync(species);

            await SpeciesWriteDbContext.SaveChangesAsync();

            return species.Id;
        }

        public async Task<Guid> SeedBreed(Guid speciesId)
        {
            var breed = Fixture.CreateBreed();

            var species = await SpeciesWriteDbContext.Species.FirstOrDefaultAsync(s => s.Id == speciesId);

            species!.AddBreed(breed);

            await SpeciesWriteDbContext.SaveChangesAsync();

            return breed.Id;
        }

        public async Task<Guid> SeedPet(Guid speciesId, Guid breedId, Guid volunteerId)
        {
            var pet = Fixture.CreatePet(speciesId, breedId);

            var volunteer = await VolunteersWriteDbContext.Volunteers
                .FirstOrDefaultAsync(s => s.Id == volunteerId);

            volunteer!.AddPet(pet);

            await VolunteersWriteDbContext.SaveChangesAsync();

            return pet.Id;
        }

        public async Task<Guid> SeedVolunteer()
        {
            var volunteer = Fixture.CreateVolunteer();

            await VolunteersWriteDbContext.Volunteers.AddAsync(volunteer);

            await VolunteersWriteDbContext.SaveChangesAsync();

            return volunteer.Id;
        }

        public async Task<Guid> SeedParticipantUser()
        {
            var role = await RoleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT);
            var userResult = Fixture.CreateParticipantUser(role!);

            await UserManager.CreateAsync(userResult.Value, userResult.Key);

            var participantAccount = new ParticipantAccount(userResult.Value);
            await ParticipantAccountManager.CreateAdminAccount(participantAccount);

            return userResult.Value.Id;
        }

        public async Task<Guid> SeedVolunteerUser()
        {
            var role = await RoleManager.FindByNameAsync(VolunteerAccount.VOLUNTEER);
            var userResult = Fixture.CreateVolunteerUser(role!);

            await UserManager.CreateAsync(userResult.Value, userResult.Key);

            var volunteerAccount = new VolunteerAccount(userResult.Value, 5);
            await VolunteerAccountManager.CreateVolunteerAccount(volunteerAccount);

            return userResult.Value.Id;
        }

        public async Task SeedVolunteers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var volunteer = Fixture.CreateVolunteer();

                await VolunteersWriteDbContext.Volunteers.AddAsync(volunteer);
            }

            await VolunteersWriteDbContext.SaveChangesAsync();
        }

        public async Task SeedSpecieses(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var species = Fixture.CreateSpecies();

                await SpeciesWriteDbContext.Species.AddAsync(species);
            }

            await SpeciesWriteDbContext.SaveChangesAsync();
        }

        public async Task SeedPets(Guid speciesId, Guid breedId, Guid volunteerId, int count)
        {
            var volunteer = await VolunteersWriteDbContext.Volunteers
                .FirstOrDefaultAsync(s => s.Id == volunteerId);

            for (int i = 0; i < count; i++)
            {
                var pet = Fixture.CreatePet(speciesId, breedId);

                volunteer!.AddPet(pet);
            }

            await VolunteersWriteDbContext.SaveChangesAsync();
        }

        public async Task SeedBreeds(Guid speciesId, int count)
        {
            var species = await SpeciesWriteDbContext.Species
                .FirstOrDefaultAsync(s => s.Id == speciesId);

            for (int i = 0; i < count; i++)
            {
                var breed = Fixture.CreateBreed();

                species!.AddBreed(breed);
            }

            await SpeciesWriteDbContext.SaveChangesAsync();
        }
    }
}
