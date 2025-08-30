using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
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
using PetFamily.VolunteerRequests.Contracts.Messaging;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Commands.TakeOnReview
{
    public class TakeRequestOnReviewHandler :
        ICommandHandler<TakeRequestOnReviewCommand>
    {
        private readonly IValidator<TakeRequestOnReviewCommand> _validator;
        private readonly IVolunteerRequestsRepository _repository;
        private readonly ILogger<TakeRequestOnReviewHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;

        public TakeRequestOnReviewHandler(
            IValidator<TakeRequestOnReviewCommand> validator,
            IVolunteerRequestsRepository repository,
            [FromKeyedServices(UnitOfWorkKeys.VolunteerRequests)] IUnitOfWork unitOfWork,
            ILogger<TakeRequestOnReviewHandler> logger,
            IPublishEndpoint publishEndpoint)
        {
            _validator = validator;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
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

            var takeOnReviewResult = request.Value.TakeOnReview(
                AdminId.Create(command.AdminId));
            if (takeOnReviewResult.IsFailure)
                return takeOnReviewResult.Error.ToErrorsList();

            try
            {
                await _publishEndpoint.Publish(new RequestTakenOnReviewEvent(
                    request.Value.Id.Value,
                    [request.Value.UserId.Value, command.AdminId]), cancellationToken);
            }
            catch (Exception ex)
            {
                return Error.Failure(
                    "discussion.module.failure", 
                    ex.Message).ToErrorsList();
            }

            await _unitOfWork.SaveChanges();

            _logger.LogInformation("request has been taken on review, discussion opened");

            return Result.Success<ErrorsList>();
        }
    }
}
