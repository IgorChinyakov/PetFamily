using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.UseCases.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.SharedVO;

namespace PetFamily.Application.Volunteers.UseCases.UpdateDetails
{
    public class UpdateDetailsHandler
    {
        private readonly IVolunteerRepository _repository;
        private readonly IValidator<UpdateDetailsCommand> _validator;
        private readonly ILogger<UpdateDetailsHandler> _logger;

        public UpdateDetailsHandler(
            IVolunteerRepository repository,
            IValidator<UpdateDetailsCommand> validator,
            ILogger<UpdateDetailsHandler> logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            UpdateDetailsCommand command,
            CancellationToken token = default)
        {
            var validationResult = await _validator.ValidateAsync(command, token);

            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var volunteerResult = await _repository.GetById(command.VolunteerId);
            if (volunteerResult.IsFailure)
                return Errors.General.ValueIsInvalid("VolunteerId").ToErrorsList();

            var details = command.Details.Select(sm => Details.Create(sm.Title, sm.Description).Value);
            volunteerResult.Value.UpdateDetailsList(details);
            var volunteerId = await _repository.Save(volunteerResult.Value, token);

            _logger.LogInformation("Volunteer's details list has been updated. Volunteer Id: {volunteerId}", volunteerId);

            return volunteerId;
        }
    }
}
