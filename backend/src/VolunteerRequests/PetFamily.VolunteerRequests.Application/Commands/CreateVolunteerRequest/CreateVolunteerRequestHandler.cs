using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.Core.Extensions;
using PetFamily.VolunteerRequests.Domain.Entities;
using PetFamily.VolunteerRequests.Domain.ValueObjects;

namespace PetFamily.VolunteerRequests.Application.Commands.CreateVolunteerRequest
{
    public class CreateVolunteerRequestHandler 
        : ICommandHandler<Guid, CreateVolunteerRequestCommand>
    {
        private readonly IValidator<CreateVolunteerRequestCommand> _validator;
        private readonly ILogger<CreateVolunteerRequestHandler> _logger;

        public CreateVolunteerRequestHandler(
            IValidator<CreateVolunteerRequestCommand> validator, 
            ILogger<CreateVolunteerRequestHandler> logger)
        {
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            CreateVolunteerRequestCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var volunteerRequest = VolunteerRequest
                .Create(UserId.Create(command.UserId), command.VolunteerInformation);


        }
    }
}
