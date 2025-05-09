using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Specieses.Application.Specieses.Commands.Create;

namespace PetFamily.IntegrationTests.Specieses
{
    public class CreateSpeciesTests : TestsBase
    {
        private readonly ICommandHandler<Guid, CreateSpeciesCommand> _sut;

        public CreateSpeciesTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateSpeciesCommand>>();
        }

        [Fact]
        public async Task CreateSpecies_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            var command = Fixture.CreateCreateSpeciesCommand();

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteers = await SpeciesReadDbContext.Species.ToListAsync();

            volunteers.Should().NotBeNull();
            volunteers.Should().HaveCount(1);
        }
    }
}
