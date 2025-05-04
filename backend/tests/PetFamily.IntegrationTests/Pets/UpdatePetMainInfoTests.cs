using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateDetails;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Pets.Commands.UpdateMainInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Pets
{
    public class UpdatePetMainInfoTests : TestsBase
    {
        private readonly ICommandHandler<UpdatePetMainInfoCommand> _sut;

        public UpdatePetMainInfoTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<UpdatePetMainInfoCommand>>();
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
            var command = Fixture.CreateUpdatePetMainInfoCommand(volunteerId, petId, speciesId, breedId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            var pet = await ReadDbContext
                .Pets.FirstOrDefaultAsync(v => v.Id == command.PetId, cancellationToken);

            pet!.Address.Should().Be(command.Address);
            pet!.Birthday.Should().BeCloseTo(command.Birthday, TimeSpan.FromMicroseconds(100));
            pet!.BreedId.Should().Be(command.BreedId);
            pet!.SpeciesId.Should().Be(command.SpeciesId);
            pet!.PhoneNumber.Should().Be(command.PhoneNumber);
            pet!.Color.Should().Be(command.Color);
            pet!.CreationDate.Should().BeCloseTo(command.CreationDate, TimeSpan.FromMicroseconds(100));
            pet!.Description.Should().Be(command.Description);
            pet!.HealthInformation.Should().Be(command.HealthInformation);
            pet!.Height.Should().Be(command.Height);
            pet!.Weight.Should().Be(command.Weight);
            pet!.IsSterilized.Should().Be(command.IsSterilized);
            pet!.IsVaccinated.Should().Be(command.IsVaccinated);
            pet!.NickName.Should().Be(command.NickName);
        }
    }
}
