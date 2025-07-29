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
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Commands.Close
{
    public class CloseDiscussionHandler : ICommandHandler<CloseDiscussionCommand>
    {
        private readonly IValidator<CloseDiscussionCommand> _validator;
        private readonly ILogger<CloseDiscussionHandler> _logger;
        private readonly IDiscussionsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CloseDiscussionHandler(
            IValidator<CloseDiscussionCommand> validator,
            ILogger<CloseDiscussionHandler> logger,
            IDiscussionsRepository repository,
            [FromKeyedServices(UnitOfWorkKeys.Discussions)] IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            CloseDiscussionCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorsList();

            var discussionId = DiscussionId.Create(command.DiscussionId);

            var discussion = await _repository.GetById(discussionId);
            if (discussion.IsFailure)
                return discussion.Error.ToErrorsList();

            discussion.Value.Close();

            await _unitOfWork.SaveChanges();

            _logger.LogInformation(
                "Discussion with relation id = {id} has been closed", discussion.Value.RelationId);

            return Result.Success<ErrorsList>();
        }
    }
}
