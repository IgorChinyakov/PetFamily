using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Pets.Queries.GetById;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.IntegrationTests.Pets
{
    public class GetPetByIdTests : TestsBase
    {
        private readonly IQueryHandlerWithResult<PetDto, GetPetByIdQuery> _sut;

        public GetPetByIdTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<IQueryHandlerWithResult<PetDto, GetPetByIdQuery>>();
        }

        [Fact]
        public async Task GetPetById_should_return_pet_with_given_id()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            var petId = await SeedPet(speciesId, breedId, volunteerId);

            var query = new GetPetByIdQuery(petId);

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(petId);
        }

        [Fact]
        public async Task Get_not_existing_pet_should_return_not_found()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);
            var petId = await SeedPet(speciesId, breedId, volunteerId);

            var query = new GetPetByIdQuery(Guid.NewGuid());

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().ContainSingle(e => e.Type == ErrorType.NotFound);
        }
    }
}
