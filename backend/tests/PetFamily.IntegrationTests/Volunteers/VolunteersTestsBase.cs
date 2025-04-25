using AutoFixture;
using Microsoft.AspNetCore.Identity.Data;
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

namespace PetFamily.IntegrationTests.Volunteers
{
    public class VolunteersTestsBase : 
        IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
    {
        protected readonly WriteDbContext WriteDbContext;
        protected readonly IReadDbContext ReadDbContext;
        protected readonly IServiceScope Scope;
        protected readonly IFixture Fixture;
        protected readonly IntegrationTestsWebFactory Factory;

        public VolunteersTestsBase(
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
    }
}
