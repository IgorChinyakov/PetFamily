using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Pets.Commands.UpdateStatus;

namespace PetFamily.IntegrationTests.Pets
{
    public class UpdatePetStatusTests : TestsBase
    {
        private readonly ICommandHandler<UpdatePetStatusCommand> _sut;

        public UpdatePetStatusTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<UpdatePetStatusCommand>>();
        }

        [Fact]
        public async Task UpdateMainInfo_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            var petId = await SeedPet(speciesId, breedId, volunteerId);
            var command = Fixture.CreateUpdatePetStatusCommand(volunteerId, petId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            var pet = await VolunteersReadDbContext
                .Pets.FirstOrDefaultAsync(v => v.Id == command.PetId, cancellationToken);

            pet!.Status.Should().Be(command.Status);
        }
    }
}
