using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.VolunteerRequests.Application.Features.Queries.GetSubmittedWithPagination;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using PetFamily.Volunteers.Application.Pets.Queries.GetFilteredWithPagiation;
using PetFamily.Volunteers.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.VolunteerRequests
{
    public class GetSubmittedRequestsTests : TestsBase
    {
        private readonly IQueryHandler<PagedList<VolunteerRequestDto>, GetSubmittedRequestsQuery> _sut;

        public GetSubmittedRequestsTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<IQueryHandler<PagedList<VolunteerRequestDto>, GetSubmittedRequestsQuery>>();
        }

        [Fact]
        public async Task GetSubmittedRequests_should_return_only_submitted_requests()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            await SeedParticipantUsers(10);

            var users = await UserManager.GetUsersInRoleAsync(ParticipantAccount.PARTICIPANT);

            for(int i = 0; i < 5; i++)
                await SeedVolunteerRequest(users[i].Id);

            for (int i = 5; i < 10; i++)
                await SeedOnReviewVolunteerRequest(users[i].Id, Guid.NewGuid());

            var query = new GetSubmittedRequestsQuery(1, 20);

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.Items.Count.Should().Be(5);
        }

        [Fact]
        public async Task GetSubmittedRequests_should_return_given_count_of_submitted_requests()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            await SeedParticipantUsers(10);

            var users = await UserManager.GetUsersInRoleAsync(ParticipantAccount.PARTICIPANT);

            for (int i = 0; i < 5; i++)
                await SeedVolunteerRequest(users[i].Id);

            for (int i = 5; i < 10; i++)
                await SeedOnReviewVolunteerRequest(users[i].Id, Guid.NewGuid());

            var query = new GetSubmittedRequestsQuery(1, 4);

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.Items.Count.Should().Be(4);
        }
    }
}
