using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteerById;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.IntegrationTests.Volunteers
{
    public class GetVolunteerByIdTests : TestsBase
    {
        private readonly IQueryHandlerWithResult<VolunteerDto, GetVolunteerByIdQuery> _sut;

        public GetVolunteerByIdTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<IQueryHandlerWithResult<VolunteerDto, GetVolunteerByIdQuery>>();
        }

        [Fact]
        public async Task GetVolunteerById_should_return_volunteer_with_given_id()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var query = new GetVolunteerByIdQuery(volunteerId);

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(volunteerId);
        }

        [Fact]
        public async Task Get_not_existing_volunteer_should_return_not_found()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var query = new GetVolunteerByIdQuery(Guid.NewGuid());

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().ContainSingle(e => e.Type == ErrorType.NotFound);
        }
    }
}
