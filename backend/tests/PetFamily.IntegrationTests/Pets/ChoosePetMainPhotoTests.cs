using Docker.DotNet.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Api.Requests.Pets;
using PetFamily.Application.Abstractions;
using PetFamily.Application.EntitiesHandling.Pets.Commands.UploadPhotos;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.VolunteerContext.PetsVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Pets
{
    public class ChoosePetMainPhotoTests : TestsBase
    {
        private readonly ICommandHandler<string, ChoosePetMainPhotoCommand> _sut;

        public ChoosePetMainPhotoTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<string, ChoosePetMainPhotoCommand>>();
        }

        [Fact]
        public async Task ChooseMainPhoto_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            var petId = await SeedPet(speciesId, breedId, volunteerId);

            var volunteer = await WriteDbContext.Volunteers
                .Include(v => v.Pets).FirstOrDefaultAsync(p => p.Id == volunteerId);

            var pet = volunteer!.Pets.FirstOrDefault(p => p.Id == petId);

            var path = "path";

            pet!.AddFiles([new PetFile(FilePath.Create(path).Value)]);

            var command = new ChoosePetMainPhotoCommand(volunteerId, petId, path);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            pet!.MainPhoto.Path.Should().Be(path);
        }
    }
}
