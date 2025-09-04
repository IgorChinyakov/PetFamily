using CSharpFunctionalExtensions;
using FluentValidation;
using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Contracts.Messaging;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Commands.Approve
{
    public class ApproveRequestHandler : ICommandHandler<ApproveRequestCommand>
    {
        private readonly IVolunteerRequestsRepository _requestsRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IValidator<ApproveRequestCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;

        public ApproveRequestHandler(
            IVolunteerRequestsRepository repository,
            IValidator<ApproveRequestCommand> validator,
            [FromKeyedServices(UnitOfWorkKeys.VolunteerRequests)] IUnitOfWork unitOfWork,
            IPublishEndpoint publishEndpoint,
            [FromKeyedServices(OutboxKeys.VolunteerRequests)] IOutboxRepository outboxRepository)
        {
            _requestsRepository = repository;
            _validator = validator;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
            _outboxRepository = outboxRepository;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            ApproveRequestCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var requestId = RequestId.Create(command.RequestId);
            var request = await _requestsRepository.GetById(requestId);
            if (request.IsFailure)
                return request.Error.ToErrorsList();

            if (request.Value.AdminId != AdminId.Create(command.AdminId))
                return Error.AccessDenied(
                    "request.by.another.admin",
                    "Can't approve request which is on review by another admin").ToErrorsList();

            request.Value.Approve();

            await _outboxRepository.Add(new RequestApprovedEvent(request.Value.UserId.Value), cancellationToken);

            await _unitOfWork.SaveChanges();

            return Result.Success<ErrorsList>();
        }
    }
}
