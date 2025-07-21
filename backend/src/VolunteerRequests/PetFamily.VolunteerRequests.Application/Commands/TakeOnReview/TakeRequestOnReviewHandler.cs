using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.Discussions.Contracts;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Commands.TakeOnReview
{
    public class TakeRequestOnReviewHandler :
        ICommandHandler<TakeRequestOnReviewCommand>
    {
        private readonly IValidator<TakeRequestOnReviewCommand> _validator;
        private readonly IAccountsContract _accountsContract;
        private readonly IDiscussionsContract _discussionsContract;
        private readonly IVolunteerRequestsRepository _repository;
        private readonly ILogger<TakeRequestOnReviewHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TakeRequestOnReviewHandler(
            IValidator<TakeRequestOnReviewCommand> validator,
            IAccountsContract accountsContract,
            IVolunteerRequestsRepository repository,
            [FromKeyedServices(UnitOfWorkKeys.VolunteerRequests)] IUnitOfWork unitOfWork,
            ILogger<TakeRequestOnReviewHandler> logger,
            IDiscussionsContract discussionsContract)
        {
            _validator = validator;
            _accountsContract = accountsContract;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _discussionsContract = discussionsContract;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            TakeRequestOnReviewCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var request = await _repository.GetById(RequestId.Create(command.RequestId));
            if (request.IsFailure)
                return request.Error.ToErrorsList();

            var discussion = await _discussionsContract.CreateDiscussion(
                new CreateDiscussionRequest(
                    request.Value.Id.Value, 
                    [request.Value.UserId.Value, command.AdminId]), 
                cancellationToken);
            if(discussion.IsFailure)
                return discussion.Error;

            var takeOnReviewResult = request.Value.TakeOnReview(
                AdminId.Create(command.AdminId), 
                DiscussionId.Create(discussion.Value));
            if(takeOnReviewResult.IsFailure)
                return takeOnReviewResult.Error.ToErrorsList();

            await _unitOfWork.SaveChanges();

            _logger.LogInformation("request has been taken on review, discussion opened");

            return Result.Success<ErrorsList>();
        }
    }
}
