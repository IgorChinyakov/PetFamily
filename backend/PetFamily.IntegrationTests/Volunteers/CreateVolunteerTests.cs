using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;

namespace PetFamily.IntegrationTests.Volunteers
{
    public class CreateVolunteerTests
    {
        private readonly IntegrationTestsWebFactory _factory;

        public CreateVolunteerTests(IntegrationTestsWebFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test1()
        {
            var scope = _factory.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IReadDbContext>();
            var pets = await dbContext.Pets.ToListAsync();

            pets.Should().BeEmpty();
        }
    }
}