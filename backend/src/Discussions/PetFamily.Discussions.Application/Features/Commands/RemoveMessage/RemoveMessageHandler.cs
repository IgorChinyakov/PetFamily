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

namespace PetFamily.Discussions.Application.Features.Commands.RemoveMessage
{
    public class RemoveMessageHandler : ICommandHandler<RemoveMessageCommand>
    {
        private readonly IValidator<RemoveMessageCommand> _validator;
        private readonly ILogger<RemoveMessageHandler> _logger;
        private readonly IDiscussionsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveMessageHandler(
            IValidator<RemoveMessageCommand> validator,
            ILogger<RemoveMessageHandler> logger,
            IDiscussionsRepository repository,
            [FromKeyedServices(UnitOfWorkKeys.Discussions)] IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            RemoveMessageCommand command,
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

            var userId = UserId.Create(command.UserId);
            var messageId = MessageId.Create(command.MessageId);

            var removementResult = discussion.Value.RemoveMessage(messageId, userId);
            if (removementResult.IsFailure)
                return removementResult.Error.ToErrorsList();

            await _unitOfWork.SaveChanges();

            _logger.LogInformation(
                "Message with id = {messageId} has been remove from discussion with relation id = {discussionId}",
                messageId.Value, discussion.Value.RelationId);

            return Result.Success<ErrorsList>();
        }
    }
}
