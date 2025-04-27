using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.EntitiesHandling.Breeds.Commands.Create;
using PetFamily.Application.EntitiesHandling.Pets.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Breeds
{
    public class CreateBreedTests : TestsBase
    {
        private readonly ICommandHandler<Guid, CreateBreedCommand> _sut;

        public CreateBreedTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateBreedCommand>>();
        }

        [Fact]
        public async Task CreateBreed_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var speciesId = await SeedSpecies();

            var command = Fixture.CreateCreateBreedCommand(speciesId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pet = await ReadDbContext.Breeds.FirstOrDefaultAsync(p => p.SpeciesId == command.SpeciesId);

            pet.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateBreed_with_not_existing_species_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var command = Fixture.CreateCreateBreedCommand(Guid.NewGuid());

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var pet = await ReadDbContext.Pets.FirstOrDefaultAsync(p => p.SpeciesId == command.SpeciesId);

            pet.Should().BeNull();
        }
    }
}
