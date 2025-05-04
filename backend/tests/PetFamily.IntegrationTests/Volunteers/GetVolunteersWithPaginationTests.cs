using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteerById;
using PetFamily.Application.Models;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Volunteers
{
    public class GetVolunteersWithPaginationTests : TestsBase
    {
        private readonly IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery> _sut;

        public GetVolunteersWithPaginationTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>>();
        }

        [Fact]
        public async Task GetVolunteers_should_return_array_with_correct_number_of_items()
        {
            // Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            await SeedVolunteers(20);

            var query = new GetVolunteersWithPaginationQuery(2, 2);

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
