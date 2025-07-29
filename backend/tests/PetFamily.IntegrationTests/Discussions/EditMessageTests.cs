using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Features.Commands.AddMessage;
using PetFamily.Discussions.Application.Features.Commands.EditMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Discussions
{
    public class EditMessageTests : TestsBase
    {
        private readonly ICommandHandler<EditMessageCommand> _sut;

        public EditMessageTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<EditMessageCommand>>();
        }

        [Fact]
        public async Task EditMessage_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var discussion = await SeedDiscussion(userId, adminId, requestId);
            var messageId = await SeedMessageToDiscussion(discussion.Id.Value, userId, "text");
            var command = Fixture.CreateEditMessageCommand(discussion.Id.Value, userId, messageId, "edited text");

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            var discussionDto = await DiscussionsReadDbContext.Discussions
                .Include(d => d.MessageDtos).FirstOrDefaultAsync();

            discussionDto!.MessageDtos.FirstOrDefault()!.Text.Should().Be("edited text");
        }

        [Fact]
        public async Task EditMessage_in_closed_discussion_should_be_failure()
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
            var command = Fixture.CreateEditMessageCommand(discussion.Id.Value, userId, messageId, "edited text");

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var discussionDto = await DiscussionsReadDbContext.Discussions
                .Include(d => d.MessageDtos).FirstOrDefaultAsync();

            discussionDto!.MessageDtos.FirstOrDefault()!.Text.Should().Be("text");
        }

        [Fact]
        public async Task EditMessage_with_invalid_user_id_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var discussion = await SeedDiscussion(userId, adminId, requestId);
            var messageId = await SeedMessageToDiscussion(discussion.Id.Value, userId, "text");
            var command = Fixture.CreateEditMessageCommand(discussion.Id.Value, Guid.NewGuid(), messageId, "edited text");

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var discussionDto = await DiscussionsReadDbContext.Discussions
                .Include(d => d.MessageDtos).FirstOrDefaultAsync();

            discussionDto!.MessageDtos.FirstOrDefault()!.Text.Should().Be("text");
        }
    }
}
