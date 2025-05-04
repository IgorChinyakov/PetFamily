using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Api.Controllers;
using PetFamily.Application.EntitiesHandling;
using PetFamily.Application.EntitiesHandling.Breeds.Commands.Delete;
using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var pets = await ReadDbContext
                .Breeds.ToListAsync();

            pets.Should().BeNullOrEmpty();
        }
    }
}
