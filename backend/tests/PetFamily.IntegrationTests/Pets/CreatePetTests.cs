using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Pets.Commands.Create;

namespace PetFamily.IntegrationTests.Pets
{
    public class CreatePetTests : TestsBase
    {
        private readonly ICommandHandler<Guid, CreatePetCommand> _sut;

        public CreatePetTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreatePetCommand>>();
        }

        [Fact]
        public async Task CreatePet_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);

            var command = Fixture.CreateCreatePetCommand(volunteerId, speciesId, breedId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pet = await VolunteersReadDbContext.Pets.FirstOrDefaultAsync(p => p.VolunteerId == command.VolunteerId);

            pet.Should().NotBeNull();
        }

        [Fact]
        public async Task CreatePet_with_not_existing_volunteer_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);

            var command = Fixture.CreateCreatePetCommand(Guid.NewGuid(), speciesId, breedId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var pet = await VolunteersReadDbContext.Pets.FirstOrDefaultAsync(p => p.VolunteerId == command.VolunteerId);

            pet.Should().BeNull();
        }
    }
}
