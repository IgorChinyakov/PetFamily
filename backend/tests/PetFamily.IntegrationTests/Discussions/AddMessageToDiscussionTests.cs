using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Features.Commands.AddMessage;
using PetFamily.Discussions.Application.Features.Commands.Close;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Discussions
{
    public class AddMessageToDiscussionTests : TestsBase
    {
        private readonly ICommandHandler<AddMessageToDiscussionCommand> _sut;

        public AddMessageToDiscussionTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<AddMessageToDiscussionCommand>>();
        }

        [Fact]
        public async Task AddMessageToDiscussion_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var discussion = await SeedDiscussion(userId, adminId, requestId);
            var command = Fixture.CreateAddMessageToDiscussionCommand(discussion.Id.Value, userId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            var discussionDto = await DiscussionsReadDbContext.Discussions
                .Include(d => d.MessageDtos).FirstOrDefaultAsync();

            discussionDto!.MessageDtos.Count().Should().Be(1);
        }

        [Fact]
        public async Task AddMessageToDiscussion_with_closed_status_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var discussion = await SeedClosedDiscussion(userId, adminId, requestId);
            var command = Fixture.CreateAddMessageToDiscussionCommand(discussion, userId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var discussionDto = await DiscussionsReadDbContext.Discussions
                .Include(d => d.MessageDtos).FirstOrDefaultAsync();

            discussionDto!.MessageDtos.Count().Should().Be(0);
        }

        [Fact]
        public async Task AddMessageToDiscussion_by_invalid_user_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var discussion = await SeedDiscussion(userId, adminId, requestId);
            var command = Fixture.CreateAddMessageToDiscussionCommand(discussion.Id.Value, Guid.NewGuid());

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var discussionDto = await DiscussionsReadDbContext.Discussions
                .Include(d => d.MessageDtos).FirstOrDefaultAsync();

            discussionDto!.MessageDtos.Count().Should().Be(0);
        }
    }
}
