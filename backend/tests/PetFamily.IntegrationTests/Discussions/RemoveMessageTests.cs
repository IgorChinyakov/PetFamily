using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Features.Commands.EditMessage;
using PetFamily.Discussions.Application.Features.Commands.RemoveMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Discussions
{
    public class RemoveMessageTests : TestsBase
    {
        private readonly ICommandHandler<RemoveMessageCommand> _sut;

        public RemoveMessageTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<RemoveMessageCommand>>();
        }

        [Fact]
        public async Task RemoveMessage_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var discussion = await SeedDiscussion(userId, adminId, requestId);
            var messageId = await SeedMessageToDiscussion(discussion.Id.Value, userId, "text");
            var command = Fixture.CreateRemoveMessageCommand(discussion.Id.Value, userId, messageId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            var discussionDto = await DiscussionsReadDbContext.Discussions
                .Include(d => d.MessageDtos).FirstOrDefaultAsync();

            discussionDto!.MessageDtos.Should().BeEmpty();
        }

        [Fact]
        public async Task RemoveMessage_from_closed_discussion_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var discussion = await SeedDiscussion(userId, adminId, requestId);
            var messageId = await SeedMessageToDiscussion(discussion.Id.Value, userId, "text");
            discussion.Close();
            await DiscussionsWriteDbContext.SaveChangesAsync();
            var command = Fixture.CreateRemoveMessageCommand(discussion.Id.Value, userId, messageId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var discussionDto = await DiscussionsReadDbContext.Discussions
                .Include(d => d.MessageDtos).FirstOrDefaultAsync();

            discussionDto!.MessageDtos.Should().NotBeEmpty();
        }

        [Fact]
        public async Task RemoveMessage_with_invalid_user_id_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var discussion = await SeedDiscussion(userId, adminId, requestId);
            var messageId = await SeedMessageToDiscussion(discussion.Id.Value, userId, "text");
            discussion.Close();
            await DiscussionsWriteDbContext.SaveChangesAsync();
            var command = Fixture.CreateRemoveMessageCommand(discussion.Id.Value, Guid.NewGuid(), messageId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var discussionDto = await DiscussionsReadDbContext.Discussions
                .Include(d => d.MessageDtos).FirstOrDefaultAsync();

            discussionDto!.MessageDtos.Should().NotBeEmpty();
        }
    }
}
