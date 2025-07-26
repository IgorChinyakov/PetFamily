using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.VolunteerRequests.Application.Features.Queries.GetRequestsByAdminId;
using PetFamily.VolunteerRequests.Application.Features.Queries.GetRequestsByuserId;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.VolunteerRequests
{
    public class GetRequestsByAdminIdTests : TestsBase
    {
        private readonly IQueryHandler<PagedList<VolunteerRequestDto>, GetRequestsByAdminIdQuery> _sut;

        public GetRequestsByAdminIdTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<IQueryHandler<PagedList<VolunteerRequestDto>, GetRequestsByAdminIdQuery>>();
        }

        [Fact]
        public async Task GetRequestsByUserId_should_return_requests_with_given_status()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var firstAdminId = await SeedAdminUser();
            var secondAdminId = await SeedAdminUser();

            for (int i = 0; i < 3; i++)
                await SeedOnReviewVolunteerRequest(userId, firstAdminId);

            for (int i = 3; i < 10; i++)
                await SeedOnReviewVolunteerRequest(userId, secondAdminId);

            var query = new GetRequestsByAdminIdQuery(secondAdminId, 1, 20, RequestStatusDto.OnReview);

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.Items.Count.Should().Be(7);
        }

        [Fact]
        public async Task GetRequestsByUserId_should_return_given_count_of_requests_with_given_status()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();

            for (int i = 0; i < 3; i++)
                await SeedRevisionRequiredVolunteerRequest(userId, adminId);

            for (int i = 3; i < 10; i++)
                await SeedOnReviewVolunteerRequest(userId, adminId);

            var query = new GetRequestsByAdminIdQuery(adminId, 1, 6, RequestStatusDto.RevisionRequired);

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.Items.Count.Should().Be(3);
        }
    }
}
