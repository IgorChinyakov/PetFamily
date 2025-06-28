using CSharpFunctionalExtensions;
using FluentAssertions;
using PetFamily.Discussions.Domain.Entities;
using PetFamily.Discussions.Domain.ValueObjects.Discussion;
using PetFamily.Discussions.Domain.ValueObjects.Message;
using PetFamily.Discussions.Domain.ValueObjects.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.UnitTests
{
    public class DiscussionTests
    {
        [Fact]
        public void Create_Discussion_Should_Return_Success()
        {
            // arrange
            List<UserId> userIds = [UserId.New(), UserId.New()];

            // act
            var discussion = Discussion.Create(userIds, RelationId.New());

            // assert
            discussion.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Create_Discussion_With_Invalid_Users_Count_Should_Return_Failure()
        {
            // arrange
            List<UserId> userIds = [UserId.New(), UserId.New(), UserId.New()];

            // act
            var discussion = Discussion.Create(userIds, RelationId.New());

            // assert
            discussion.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Add_Message_Should_Return_Success()
        {
            // arrange
            var userId = UserId.New();
            List<UserId> userIds = [userId, UserId.New()];

            var discussion = Discussion.Create(userIds, RelationId.New()).Value;

            var text = Text.Create("some text").Value;
            // act
            var result = discussion.AddMessage(text, userId);

            // assert
            result.IsSuccess.Should().BeTrue();
            discussion.Messages.Count.Should().Be(1);
        }

        [Fact]
        public void Add_Message_With_Invalid_User_Id_Should_Return_Failure()
        {
            // arrange
            var userId = UserId.New();
            List<UserId> userIds = [userId, UserId.New()];

            var discussion = Discussion.Create(userIds, RelationId.New()).Value;

            var text = Text.Create("some text").Value;

            // act
            var result = discussion.AddMessage(text, UserId.New());

            // assert
            result.IsSuccess.Should().BeFalse();
            discussion.Messages.Count.Should().Be(0);
        }

        [Fact]
        public void Remove_Message_Should_Return_Success()
        {
            // arrange
            var userId = UserId.New();
            List<UserId> userIds = [userId, UserId.New()];

            var discussion = Discussion.Create(userIds, RelationId.New()).Value;

            var text = Text.Create("some text").Value;

            var message = discussion.AddMessage(text, userId);

            // act
            discussion.RemoveMessage(message.Value, userId);

            // assert
            discussion.Messages.Count.Should().Be(0);
        }

        [Fact]
        public void Remove_Message_With_Invalid_Message_Id_Should_Return_Failure()
        {
            // arrange
            var userId = UserId.New();
            List<UserId> userIds = [userId, UserId.New()];

            var discussion = Discussion.Create(userIds, RelationId.New()).Value;

            var text = Text.Create("some text").Value;

            var message = discussion.AddMessage(text, userId);

            // act
            var result = discussion.RemoveMessage(MessageId.New(), userId);

            // assert
            result.IsSuccess.Should().BeFalse();
            discussion.Messages.Count.Should().Be(1);
        }

        [Fact]
        public void Edit_Message_Should_Return_Success()
        {
            // arrange
            var userId = UserId.New();
            List<UserId> userIds = [userId, UserId.New()];

            var discussion = Discussion.Create(userIds, RelationId.New()).Value;

            var text = Text.Create("some text").Value;

            var message = discussion.AddMessage(text, userId);

            var editedText = Text.Create("some edited text").Value;

            // act
            var result = discussion.EditMessage(editedText, message.Value, userId);

            // assert
            var finalMessage = discussion.Messages.FirstOrDefault(m => m.Id == message.Value)!;
            result.IsSuccess.Should().BeTrue();
            finalMessage.Text.Should().Be(editedText);
            finalMessage.IsEdited.Value.Should().Be(true);
            discussion.Messages.Count.Should().Be(1);
        }

        [Fact]
        public void Close_Discussion_Should_Be_Success()
        {
            // arrange
            var userId = UserId.New();
            List<UserId> userIds = [userId, UserId.New()];

            var discussion = Discussion.Create(userIds, RelationId.New()).Value;

            // act
            discussion.Close();

            // assert
            discussion.Status.Value.Should().Be(DiscussionStatus.Status.Closed);
        }
    }
}
