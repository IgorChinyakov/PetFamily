using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Pets.Commands.Move;

namespace PetFamily.IntegrationTests.Pets
{
    public class MovePetTests : TestsBase
    {
        private readonly ICommandHandler<MovePetCommand> _sut;

        public MovePetTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<MovePetCommand>>();
        }

        [Fact]
        public async Task MovePet_when_current_position_bigger_than_position_to_move_should_succeed()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            await SeedPets(speciesId, breedId, volunteerId, 10);

            var volunteer = await VolunteersWriteDbContext.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteerId);
            var pets = volunteer!.Pets;

            var command = Fixture.CreateMovePetCommand(volunteerId, pets[2].Id, 9);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            pets[2].Position.Value.Should().Be(9);
            for (int i = 3; i <= 8; i++)
                pets[i].Position.Value.Should().Be(i);
        }

        [Fact]
        public async Task MovePet_when_current_position_lower_than_position_to_move_should_succeed()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            await SeedPets(speciesId, breedId, volunteerId, 10);

            var volunteer = await VolunteersWriteDbContext.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteerId);
            var pets = volunteer!.Pets;

            var command = Fixture.CreateMovePetCommand(volunteerId, pets[8].Id, 3);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            pets[8].Position.Value.Should().Be(3);
            for (int i = 2; i < 8; i++)
                pets[i].Position.Value.Should().Be(i+2);
        }

        [Fact]
        public async Task MovePet_when_current_position_equal_position_to_move_should_succeed()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            await SeedPets(speciesId, breedId, volunteerId, 10);

            var volunteer = await VolunteersWriteDbContext.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteerId);
            var pets = volunteer!.Pets;

            var command = Fixture.CreateMovePetCommand(volunteerId, pets[8].Id, 9);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            pets[8].Position.Value.Should().Be(9);
        }

        [Fact]
        public async Task MovePet_when_position_to_move_bigger_than_pets_count_should_succeed()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            await SeedPets(speciesId, breedId, volunteerId, 10);

            var volunteer = await VolunteersWriteDbContext.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteerId);
            var pets = volunteer!.Pets;

            var command = Fixture.CreateMovePetCommand(volunteerId, pets[2].Id, 78);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            pets[2].Position.Value.Should().Be(10);
        }

        [Fact]
        public async Task MovePet_when_to_first_position_should_succeed()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            await SeedPets(speciesId, breedId, volunteerId, 10);

            var volunteer = await VolunteersWriteDbContext.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteerId);
            var pets = volunteer!.Pets;

            var command = Fixture.CreateMovePetCommand(volunteerId, pets[6].Id, 1);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            pets[6].Position.Value.Should().Be(1);
            for (int i = 0; i < 6; i++)
                pets[i].Position.Value.Should().Be(i + 2);
        }
    }
}
