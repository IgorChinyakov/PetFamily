using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Domain.ValueObjects.Discussion;
using PetFamily.Discussions.Domain.ValueObjects.Message;
using PetFamily.Discussions.Domain.ValueObjects.Shared;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Commands.EditMessage
{
    public class EditMessageHandler : ICommandHandler<EditMessageCommand>
    {
        private readonly IValidator<EditMessageCommand> _validator;
        private readonly ILogger<EditMessageHandler> _logger;
        private readonly IDiscussionsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EditMessageHandler(
            IValidator<EditMessageCommand> validator,
            ILogger<EditMessageHandler> logger,
            IDiscussionsRepository repository,
            [FromKeyedServices(UnitOfWorkKeys.Discussions)] IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            EditMessageCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorsList();

            var discussionId = DiscussionId.Create(command.DiscussionId);

            var discussion = await _repository.GetById(discussionId);
            if (discussion.IsFailure)
                return discussion.Error.ToErrorsList();

            if (discussion.Value.Status.Value == DiscussionStatus.Status.Closed)
                return Error.AccessDenied(
                    "discussion.closed",
                    "Can't edit message in closed discussion").ToErrorsList();

            var text = Text.Create(command.EditedMessage).Value;
            var userId = UserId.Create(command.UserId);
            var messageId = MessageId.Create(command.MessageId);

            var editMessageResult = discussion.Value.EditMessage(text, messageId, userId);
            if (editMessageResult.IsFailure)
                return editMessageResult.Error.ToErrorsList();

            await _unitOfWork.SaveChanges();

            _logger.LogInformation(
                "Message with id = {messageId} has been edited to discussion with relation id = {discussionId}", 
                messageId.Value, discussion.Value.RelationId);

            return Result.Success<ErrorsList>();
        }
    }
}
