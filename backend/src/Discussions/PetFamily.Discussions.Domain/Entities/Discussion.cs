using CSharpFunctionalExtensions;
using PetFamily.Discussions.Domain.ValueObjects.Discussion;
using PetFamily.Discussions.Domain.ValueObjects.Message;
using PetFamily.Discussions.Domain.ValueObjects.Shared;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Domain.Entities
{
    public class Discussion
    {
        public const int DISCUSSION_MEMBERS_COUNT = 2;

        private readonly List<UserId> _userIds = [];
        private readonly List<Message> _messages = [];

        //ef core
        private Discussion()
        {
        }

        public DiscussionId Id { get; set; }

        public IReadOnlyList<Message> Messages => _messages;

        public IReadOnlyList<UserId> UserIds => _userIds;

        public RelationId RelationId { get; set; }

        public DiscussionStatus Status { get; set; }

        private Discussion(
            DiscussionId discussionId,
            List<UserId> userIds,
            RelationId relationId,
            DiscussionStatus status)
        {
            Id = discussionId;
            _userIds = userIds;
            RelationId = relationId;
            Status = status;
        }

        public static Result<Discussion, Error> Create(
            IEnumerable<UserId> userIds,
            RelationId relationId)
        {
            if (userIds.Count() != DISCUSSION_MEMBERS_COUNT)
                return Errors.General.ValueIsInvalid("Users count");

            return new Discussion(
                DiscussionId.New(), 
                userIds.ToList(), 
                relationId, 
                DiscussionStatus.Create(DiscussionStatus.Status.Open));
        }

        public Result<MessageId, Error> AddMessage(
             Text text,
             UserId userId)
        {
            if (UserIds.FirstOrDefault(u => u == userId) == null)
                return Errors.General.ValueIsInvalid("user");

            var message = new Message(
                MessageId.New(),
                userId,
                text,
                CreationDate.Create(DateTime.UtcNow).Value,
                IsEdited.Create(false));

            _messages.Add(message);

            return message.Id;
        }

        public UnitResult<Error> RemoveMessage(
             MessageId messageId,
             UserId userId)
        {
            var message = Messages.FirstOrDefault(m => m.Id == messageId && m.UserId == userId);
            if (message == null)
                return Errors.General.NotFound(messageId.Value);

            _messages.Remove(message);

            return Result.Success<Error>();
        }

        public UnitResult<Error> EditMessage(
             Text text,
             MessageId messageId,
             UserId userId)
        {
            var message = Messages.FirstOrDefault(m => m.Id == messageId && m.UserId == userId);
            if(message == null)
                return Errors.General.NotFound(messageId.Value);

            message.EditText(text);
            message.ChangeIsEdit(IsEdited.Create(true));

            return Result.Success<Error>();
        }

        public void Close()
           => Status = DiscussionStatus.Create(DiscussionStatus.Status.Closed);
    }
}
