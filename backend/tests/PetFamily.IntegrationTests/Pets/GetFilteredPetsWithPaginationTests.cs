using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Pets.Queries.GetFilteredWithPagiation;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.IntegrationTests.Pets
{
    public class GetFilteredPetsWithPaginationTests : TestsBase
    {
        private readonly IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery> _sut;

        public GetFilteredPetsWithPaginationTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>>();
        }

        [Fact]
        public async Task GetPetsWithPagination_should_return_given_count_of_pets()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var breedId = await SeedBreed(speciesId);

            await SeedPets(speciesId, breedId, volunteerId, 20);

            var query = new GetFilteredPetsWithPaginationQuery(1, 10);

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.Items.Count.Should().Be(10);
        }

        [Fact]
        public async Task GetPetsWithPagination_with_filter_should_return_given_count_of_pets()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var speciesId = await SeedSpecies();
            var firstBreedId = await SeedBreed(speciesId);
            var secondBreedId = await SeedBreed(speciesId);

            await SeedPets(speciesId, firstBreedId, volunteerId, 10);
            await SeedPets(speciesId, secondBreedId, volunteerId, 5);

            var query = new GetFilteredPetsWithPaginationQuery(1, 10, BreedId: secondBreedId);

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.Items.Count.Should().Be(5);
        }
    }
}
