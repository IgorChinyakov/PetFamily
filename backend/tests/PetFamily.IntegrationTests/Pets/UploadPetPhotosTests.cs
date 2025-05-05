using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.FileDtos;
using PetFamily.Volunteers.Application.Pets.Commands.UploadPhotos;

namespace PetFamily.IntegrationTests.Pets
{
    public class UploadPetPhotosTests : TestsBase
    {
        private readonly ICommandHandler<IReadOnlyList<string>, UploadPetPhotosCommand> _sut;

        public UploadPetPhotosTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<IReadOnlyList<string>, UploadPetPhotosCommand>>();
        }

        [Fact]
        public async Task UploadPhotos_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            Factory.SetupSuccessFilesProviderMock();

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            var petId = await SeedPet(speciesId, breedId, volunteerId);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Some test file content");
            var fileDto = new CreateFileDto(stream, "path");

            var command = new UploadPetPhotosCommand([fileDto], volunteerId, petId, "photos");

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            var pet = await VolunteersReadDbContext.Pets.FirstOrDefaultAsync(p => p.Id == petId);
            pet!.Files.Should().NotBeNullOrEmpty();
        }
    }
}
