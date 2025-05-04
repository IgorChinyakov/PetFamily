using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Delete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Core.Options;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Pets.Commands.Delete;

namespace PetFamily.IntegrationTests.Pets
{
    public class DeletePetTests : TestsBase
    {
        private readonly ICommandHandler<Guid, DeletePetCommand> _sut;

        public DeletePetTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, DeletePetCommand>>();
        }

        [Fact]
        public async Task HardDeletePet_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            var petId = await SeedPet(speciesId, breedId, volunteerId);

            var command = Fixture.CreateDeletePetCommand(volunteerId, petId, DeletionOptions.Hard);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pets = await ReadDbContext
                .Pets.ToListAsync();

            pets.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task SoftDeletePet_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            var petId = await SeedPet(speciesId, breedId, volunteerId);

            var command = Fixture.CreateDeletePetCommand(volunteerId, petId, DeletionOptions.Soft);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var pet = await ReadDbContext
                .Pets.FirstOrDefaultAsync();

            pet!.IsDeleted.Should().BeTrue();
        }
    }
}
