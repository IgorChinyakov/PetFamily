using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Commands.Update
{
    public class UpdateRequestHandler : ICommandHandler<UpdateRequestCommand>
    {
        private readonly IValidator<UpdateRequestCommand> _validator;
        private readonly ILogger<UpdateRequestHandler> _logger;
        private readonly IVolunteerRequestsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRequestHandler(
            IValidator<UpdateRequestCommand> validator,
            ILogger<UpdateRequestHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.VolunteerRequests)] IUnitOfWork unitOfWork,
            IVolunteerRequestsRepository repository)
        {
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        public async Task<UnitResult<ErrorsList>> Handle(
            UpdateRequestCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var requestId = RequestId.Create(command.RequestId);
            var request = await _repository.GetById(requestId);
            if (request.IsFailure)
                return request.Error.ToErrorsList();

            if (request.Value.UserId.Value != command.UserId)
                return Error.AccessDenied(
                    "rejected.to.update.request",
                    "Request belongs to another user").ToErrorsList();

            if (request.Value.RequestStatus.Value != RequestStatus.Status.RevisionRequired)
                return Error.AccessDenied(
                    "invalid.request.status",
                    "Request must have RevisionRequired status").ToErrorsList();

            var volunteerInformation = VolunteerInformation.Create(command.UpdatedInformation).Value;
            request.Value.UpdateVolunteerInformation(volunteerInformation);
            request.Value.TakeOnReview(request.Value.AdminId!);

            await _unitOfWork.SaveChanges();

            _logger.LogInformation("request {requestId} has been updated", request.Value.Id);

            return Result.Success<ErrorsList>();
        }
    }
}
