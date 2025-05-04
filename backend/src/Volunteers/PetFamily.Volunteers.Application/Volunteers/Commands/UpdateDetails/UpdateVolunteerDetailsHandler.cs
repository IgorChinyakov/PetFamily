using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.SharedVO;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateDetails
{
    public class UpdateVolunteerDetailsHandler :
        ICommandHandler<Guid, UpdateVolunteerDetailsCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<UpdateVolunteerDetailsCommand> _validator;
        private readonly ILogger<UpdateVolunteerDetailsHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVolunteerDetailsHandler(
            IVolunteersRepository repository,
            IValidator<UpdateVolunteerDetailsCommand> validator,
            ILogger<UpdateVolunteerDetailsHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Volunteers)] IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            UpdateVolunteerDetailsCommand command,
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
            await _unitOfWork.SaveChanges(token);

            _logger.LogInformation("Volunteer's details list has been updated. Volunteer Id: {volunteerId}", volunteerResult.Value.Id);

            return volunteerResult.Value.Id;
        }
    }
}
