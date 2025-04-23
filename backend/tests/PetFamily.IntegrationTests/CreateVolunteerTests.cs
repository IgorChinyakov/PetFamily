using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;

namespace PetFamily.IntegrationTests
{
    public class CreateVolunteerTests :
        IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
    {
        private readonly IntegrationTestsWebFactory _factory;
        private readonly IReadDbContext _readDbContext;
        private readonly IServiceScope _scope;

        public CreateVolunteerTests(IntegrationTestsWebFactory factory)
        {
            _factory = factory;
            _scope = _factory.Services.CreateScope();
            _readDbContext = _scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        }

        [Fact]
        public async Task Test1()
        {
            var pets = await _readDbContext.Pets.ToListAsync();

            pets.Should().BeEmpty();
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync()
        {
            _scope.Dispose();

            return Task.CompletedTask;
        }
    }
}