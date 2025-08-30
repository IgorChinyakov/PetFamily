using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Infrastructure.Authorization.Managers;
using PetFamily.Accounts.Infrastructure.Authorization.Seeding;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Domain.Entities;
using PetFamily.Discussions.Domain.ValueObjects.Discussion;
using PetFamily.Discussions.Domain.ValueObjects.Message;
using PetFamily.Discussions.Domain.ValueObjects.Shared;
using PetFamily.Discussions.Infrastructure.DbContexts;
using PetFamily.Specieses.Application.Database;
using PetFamily.Specieses.Infrastructure.DbContexts;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.IntegrationTests
{
    public class TestsBase :
        IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
    {
        protected readonly SpeciesWriteDbContext SpeciesWriteDbContext;
        protected readonly VolunteersWriteDbContext VolunteersWriteDbContext;
        protected readonly VolunteerRequestsWriteDbContext VolunteerRequestsWriteDbContext;
        protected readonly DiscussionsWriteDbContext DiscussionsWriteDbContext;
        protected readonly IDiscussionsReadDbContext DiscussionsReadDbContext;
        protected readonly ISpeciesReadDbContext SpeciesReadDbContext;
        protected readonly IVolunteersReadDbContext VolunteersReadDbContext;
        protected readonly IVolunteerRequestsReadDbContext VolunteerRequestsReadDbContext;
        protected readonly UserManager<User> UserManager;
        protected readonly RoleManager<Role> RoleManager;
        protected readonly IParticipantAccountManager ParticipantAccountManager;
        protected readonly IVolunteerAccountManager VolunteerAccountManager;
        protected readonly IAdminAccountManager AdminAccountManager;
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
            VolunteerRequestsReadDbContext = Scope.ServiceProvider.GetRequiredService<IVolunteerRequestsReadDbContext>();
            DiscussionsReadDbContext = Scope.ServiceProvider.GetRequiredService<IDiscussionsReadDbContext>();
            VolunteerRequestsWriteDbContext = Scope.ServiceProvider.GetRequiredService<VolunteerRequestsWriteDbContext>();
            SpeciesWriteDbContext = Scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
            VolunteersWriteDbContext = Scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
            DiscussionsWriteDbContext = Scope.ServiceProvider.GetRequiredService<DiscussionsWriteDbContext>();
            UserManager = Scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            RoleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            ParticipantAccountManager = Scope.ServiceProvider.GetRequiredService<IParticipantAccountManager>();
            VolunteerAccountManager = Scope.ServiceProvider.GetRequiredService<IVolunteerAccountManager>();
            AdminAccountManager = Scope.ServiceProvider.GetRequiredService<IAdminAccountManager>();
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            Scope.Dispose();
            await Factory.ResetDatabase();
        }

        public async Task SeedAccounts()
        {
            var seeder = Scope.ServiceProvider.GetRequiredService<AccountsSeeder>();
            await seeder.SeedAsync();
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

        public async Task<Guid> SeedMessageToDiscussion(
            Guid discussionId, Guid userId, string text)
        {
            var discussion = await DiscussionsWriteDbContext.Discussions
                .Include(d => d.Messages)
                .FirstOrDefaultAsync(d => d.Id == 
                PetFamily.Discussions.Domain.ValueObjects.Discussion.DiscussionId.Create(discussionId));

            var messageId = discussion!.AddMessage(Text.Create(text).Value, 
                PetFamily.Discussions.Domain.ValueObjects.Shared.UserId.Create(userId)).Value;

            await DiscussionsWriteDbContext.SaveChangesAsync();

            return messageId.Value;
        }

        public async Task SeedMessagesToDiscussion(
            Guid discussionId, Guid userId, int count)
        {
            var discussion = await DiscussionsWriteDbContext.Discussions
                .Include(d => d.Messages)
                .FirstOrDefaultAsync(d => d.Id == 
                PetFamily.Discussions.Domain.ValueObjects.Discussion.DiscussionId.Create(discussionId));

            for (int i = 0; i < count; i++)
            {
                discussion!.AddMessage(Text.Create("text").Value,
                PetFamily.Discussions.Domain.ValueObjects.Shared.UserId.Create(userId));
            }

            await DiscussionsWriteDbContext.SaveChangesAsync();
        }

        public async Task<Discussion> SeedDiscussion(
            Guid userId, Guid adminId, Guid relationId)
        {
            var discussion = Fixture.CreateDiscussion(userId, adminId, relationId);

            await DiscussionsWriteDbContext.Discussions.AddAsync(discussion);

            await DiscussionsWriteDbContext.SaveChangesAsync();

            return discussion;
        }

        public async Task<Guid> SeedClosedDiscussion(
            Guid userId, Guid adminId, Guid relationId)
        {
            var discussion = Fixture.CreateDiscussion(userId, adminId, relationId);
            discussion.Close();

            await DiscussionsWriteDbContext.Discussions.AddAsync(discussion);

            await DiscussionsWriteDbContext.SaveChangesAsync();

            return discussion.Id.Value;
        }

        public async Task<Guid> SeedParticipantUser()
        {
            var role = await RoleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT);
            var userResult = Fixture.CreateParticipantUser(role!);

            await UserManager.CreateAsync(userResult.Value, userResult.Key);

            var participantAccount = new ParticipantAccount(userResult.Value);
            await ParticipantAccountManager.CreateParticipantAccount(participantAccount);

            return userResult.Value.Id;
        }

        public async Task SeedParticipantUsers(int count)
        {
            var role = await RoleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT);

            for (int i = 0; i < count; i++)
            {
                var userResult = Fixture.CreateParticipantUser(role!);  
                await UserManager.CreateAsync(userResult.Value, userResult.Key);

                var participantAccount = new ParticipantAccount(userResult.Value);
                await ParticipantAccountManager.CreateParticipantAccount(participantAccount);
            }
        }

        public async Task<Guid> SeedVolunteerUser()
        {
            var role = await RoleManager.FindByNameAsync(VolunteerAccount.VOLUNTEER);
            var userResult = Fixture.CreateVolunteerUser(role!);

            await UserManager.CreateAsync(userResult.Value, userResult.Key);

            var volunteerAccount = new VolunteerAccount(userResult.Value);
            await VolunteerAccountManager.CreateVolunteerAccount(volunteerAccount);

            return userResult.Value.Id;
        }

        public async Task<Guid> SeedAdminUser()
        {
            var role = await RoleManager.FindByNameAsync(AdminAccount.ADMIN);
            var userResult = Fixture.CreateAdminUser(role!);

            await UserManager.CreateAsync(userResult.Value, userResult.Key);

            var adminAccount = new AdminAccount(userResult.Value);
            await AdminAccountManager.CreateAdminAccount(adminAccount);

            return userResult.Value.Id;
        }

        public async Task<Guid> SeedVolunteerRequest(Guid userId)
        {
            var request = Fixture.CreateVolunteerRequest(userId);
            await VolunteerRequestsWriteDbContext.VolunteerRequests.AddAsync(request);

            await VolunteerRequestsWriteDbContext.SaveChangesAsync();

            return request.Id.Value;
        }

        public async Task<Guid> SeedRejectedVolunteerRequest(Guid userId)
        {
            var request = Fixture.CreateVolunteerRequest(userId);
            await VolunteerRequestsWriteDbContext.VolunteerRequests.AddAsync(request);
            request.Reject();

            await VolunteerRequestsWriteDbContext.SaveChangesAsync();

            return request.Id.Value;
        }

        public async Task<Guid> SeedOnReviewVolunteerRequest(Guid userId, Guid adminId)
        {
            var request = Fixture.CreateVolunteerRequest(userId);
            await VolunteerRequestsWriteDbContext.VolunteerRequests.AddAsync(request);
            request.TakeOnReview(AdminId.Create(adminId));

            await VolunteerRequestsWriteDbContext.SaveChangesAsync();

            return request.Id.Value;
        }

        public async Task<Guid> SeedRevisionRequiredVolunteerRequest(Guid userId, Guid adminId)
        {
            var request = Fixture.CreateVolunteerRequest(userId);
            await VolunteerRequestsWriteDbContext.VolunteerRequests.AddAsync(request);
            request.TakeOnReview(AdminId.Create(adminId));
            request.SendForRevision(RejectionComment.Create("rejection comment").Value);

            await VolunteerRequestsWriteDbContext.SaveChangesAsync();

            return request.Id.Value;
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
