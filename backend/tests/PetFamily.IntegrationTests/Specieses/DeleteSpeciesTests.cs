using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Delete;
using PetFamily.Application.EntitiesHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetFamily.Application.EntitiesHandling.Specieses.Commands.Delete;
using FluentAssertions;

namespace PetFamily.IntegrationTests.Specieses
{
    public class DeleteSpeciesTests : TestsBase
    {
        private readonly ICommandHandler<Guid, DeleteSpeciesCommand> _sut;

        public DeleteSpeciesTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, DeleteSpeciesCommand>>();
        }

        [Fact]
        public async Task DeleteSpecies_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var speciesId = await SeedSpecies();

            var command = Fixture.CreateDeleteSpeciesCommand(speciesId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var species = await ReadDbContext
                .Species.ToListAsync();

            species.Should().BeNullOrEmpty();
        }
    }
}
