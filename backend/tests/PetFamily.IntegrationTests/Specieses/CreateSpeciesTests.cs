using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.EntitiesHandling.Specieses.Commands.Create;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Create;
using PetFamily.Core.Abstractions;
using PetFamily.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var volunteers = await ReadDbContext.Species.ToListAsync();

            volunteers.Should().NotBeNull();
            volunteers.Should().HaveCount(1);
        }
    }
}
