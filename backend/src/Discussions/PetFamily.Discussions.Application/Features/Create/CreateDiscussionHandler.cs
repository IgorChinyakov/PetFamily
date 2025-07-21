using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Domain.Entities;
using PetFamily.Discussions.Domain.ValueObjects.Discussion;
using PetFamily.Discussions.Domain.ValueObjects.Shared;
using PetFamily.SharedKernel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Create
{
    public class CreateDiscussionHandler : 
        ICommandHandler<Guid, CreateDiscussionCommand>
    {
        private readonly IValidator<CreateDiscussionCommand> _validator;
        private readonly ILogger<CreateDiscussionHandler> _logger;
        private readonly IDiscussionsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDiscussionHandler(
            IValidator<CreateDiscussionCommand> validator, 
            ILogger<CreateDiscussionHandler> logger, 
            IDiscussionsRepository repository, 
            [FromKeyedServices(UnitOfWorkKeys.Discussions)]IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            CreateDiscussionCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorsList();

            var relationId = RelationId.Create(command.RelationId);

            var existingDiscussion = await _repository.GetByRelationId(relationId);
            if (existingDiscussion.IsSuccess)
                return Errors.General.Conflict("discussion").ToErrorsList();

            var userIds = command.UserIds.Select(UserId.Create).ToList();

            var discussion = Discussion.Create(userIds, relationId);

            if (discussion.IsFailure)
                return discussion.Error.ToErrorsList();

            await _repository.Add(discussion.Value, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                "Created discussion with Id = {id}", discussion.Value.Id.Value);

            return discussion.Value.Id.Value;
        }
    }
}
