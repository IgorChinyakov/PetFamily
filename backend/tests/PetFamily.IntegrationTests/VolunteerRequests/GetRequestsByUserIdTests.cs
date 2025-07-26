using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.VolunteerRequests.Application.Features.Queries.GetRequestsByuserId;
using PetFamily.VolunteerRequests.Application.Features.Queries.GetSubmittedWithPagination;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.VolunteerRequests
{
    public class GetRequestsByUserIdTests : TestsBase
    {
        private readonly IQueryHandler<PagedList<VolunteerRequestDto>, GetRequestsByUserIdQuery> _sut;

        public GetRequestsByUserIdTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<IQueryHandler<PagedList<VolunteerRequestDto>, GetRequestsByUserIdQuery>>();
        }

        [Fact]
        public async Task GetRequestsByUserId_should_return_requests_with_given_status()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var userId = await SeedParticipantUser();

            for (int i = 0; i < 3; i++)
                await SeedVolunteerRequest(userId);

            for (int i = 3; i < 10; i++)
                await SeedOnReviewVolunteerRequest(userId, Guid.NewGuid());

            var query = new GetRequestsByUserIdQuery(userId, 1, 20);

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.Items.Count.Should().Be(10);
        }

        [Fact]
        public async Task GetRequestsByUserId_should_return_given_count_of_requests_with_given_status()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();

            for (int i = 0; i < 3; i++)
                await SeedVolunteerRequest(userId);

            for (int i = 3; i < 10; i++)
                await SeedOnReviewVolunteerRequest(userId, Guid.NewGuid());

            var query = new GetRequestsByUserIdQuery(userId, 1, 6, RequestStatusDto.OnReview);

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.Items.Count.Should().Be(6);
        }
    }
}
