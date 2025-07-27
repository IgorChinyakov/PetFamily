using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Features.Commands.RemoveMessage;
using PetFamily.Discussions.Application.Features.Queries.GetDiscussionByRelationId;
using PetFamily.Discussions.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Discussions
{
    public class GetDiscussionByRealtionIdTests : TestsBase
    {
        private readonly IQueryHandlerWithResult<DiscussionDto, GetDiscussionByRelationIdQuery> _sut;

        public GetDiscussionByRealtionIdTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<IQueryHandlerWithResult<DiscussionDto, GetDiscussionByRelationIdQuery>>();
        }

        [Fact]
        public async Task GetDiscussionByRealtionId_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var discussion = await SeedDiscussion(userId, adminId, requestId);
            await SeedMessagesToDiscussion(discussion.Id.Value, userId, 10);
            var query = new GetDiscussionByRelationIdQuery(requestId);

            //Act
            var result = await _sut.Handle(query, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.MessageDtos.Count().Should().Be(10);
        }
    }
}
