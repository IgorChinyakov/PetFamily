using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Specieses.Application.Breeds.Commands.Delete;

namespace PetFamily.IntegrationTests.Breeds
{
    public class DeleteBreedTests : TestsBase
    {
        private readonly ICommandHandler<Guid, DeleteBreedCommand> _sut;

        public DeleteBreedTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, DeleteBreedCommand>>();
        }

        [Fact]
        public async Task HardDeletePet_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);

            var command = Fixture.CreateDeleteBreedCommand(speciesId, breedId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pets = await SpeciesReadDbContext
                .Breeds.ToListAsync();

            pets.Should().BeNullOrEmpty();
        }
    }
}
