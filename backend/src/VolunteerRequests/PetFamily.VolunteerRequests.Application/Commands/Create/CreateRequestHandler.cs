using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.Core.Extensions;
using PetFamily.VolunteerRequests.Domain.Entities;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using PetFamily.Core.Abstractions.Database;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Options;
using PetFamily.VolunteerRequests.Application.Database;
using Microsoft.Extensions.Options;

namespace PetFamily.VolunteerRequests.Application.Commands.CreateVolunteerRequest
{
    public class CreateRequestHandler 
        : ICommandHandler<Guid, CreateRequestCommand>
    {
        private readonly IValidator<CreateRequestCommand> _validator;
        private readonly ILogger<CreateRequestHandler> _logger;
        private readonly IVolunteerRequestsRepository _repository;
        private readonly VolunteerRequestSettings _options;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRequestHandler(
            IValidator<CreateRequestCommand> validator,
            ILogger<CreateRequestHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.VolunteerRequests)] IUnitOfWork unitOfWork,
            IVolunteerRequestsRepository repository,
            IOptions<VolunteerRequestSettings> options)
        {
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _options = options.Value;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            CreateRequestCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var existingRequest = await _repository.GetByUserId(UserId.Create(command.UserId));
            var existingRequestStatus = existingRequest.Value.RequestStatus.Value;
            if (existingRequest.IsSuccess &&
                existingRequestStatus != RequestStatus.Status.Rejected &&
                existingRequestStatus != RequestStatus.Status.Approved)
                return Errors.General.Conflict("Volunteer request for this user").ToErrorsList();

            var information = VolunteerInformation.Create(command.VolunteerInformation).Value;

            var hasRecentRejection = await _repository.HasRecentRejection(
                UserId.Create(command.UserId), _options.DaysAfterRejectionToMakeAnotherRequest);
            if (hasRecentRejection)
                return Error.Failure(
                    "few.days.since.last.rejection", 
                    "Not enough time has passed since last rejection").ToErrorsList();

            var volunteerRequest = VolunteerRequest
                .Create(UserId.Create(command.UserId), information).Value;

            await _repository.Add(volunteerRequest);
            await _unitOfWork.SaveChanges();

            _logger.LogInformation("Volunteer request for user {userId}has been added", command.UserId);

            return volunteerRequest.Id.Value;
        }
    }
}
