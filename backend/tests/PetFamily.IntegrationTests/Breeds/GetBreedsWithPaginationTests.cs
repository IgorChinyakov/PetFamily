using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.EntitiesHandling.Breeds.Queries.GetBreedsWithPagination;
using PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Application.Models;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Breeds
{
    public class GetBreedsWithPaginationTests : TestsBase
    {
        private readonly IQueryHandler<PagedList<BreedDto>, GetBreedsWithPaginationQuery> _sut;

        public GetBreedsWithPaginationTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<IQueryHandler<PagedList<BreedDto>, GetBreedsWithPaginationQuery>>();
        }

        [Fact]
        public async Task GetVolunteers_should_return_array_with_correct_number_of_items()
        {
            // Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var speciesId = await SeedSpecies();
            await SeedBreeds(speciesId, 20);

            var query = new GetBreedsWithPaginationQuery(speciesId, 2, 2);

            // Act
            var result = await _sut.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();

            result.PageSize.Should().Be(query.PageSize);
            result.Page.Should().Be(query.Page);
            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
        }
    }
}
