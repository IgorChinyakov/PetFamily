using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.SharedVO;
using PetFamily.Volunteers.Domain.VolunteersVO;
using PetFamily.Core.Extensions;
using PetFamily.Core.Abstractions.Database;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Options;
using PetFamily.Volunteers.Application.Database;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Create
{
    public class CreateVolunteerHandler : ICommandHandler<Guid, CreateVolunteerCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<CreateVolunteerCommand> _validator;
        private readonly ILogger<CreateVolunteerHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVolunteerHandler(
            IVolunteersRepository repository,
            IValidator<CreateVolunteerCommand> validator,
            ILogger<CreateVolunteerHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Volunteers)]IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            CreateVolunteerCommand command,
            CancellationToken token = default)
        {
            var result = await _validator.ValidateAsync(command, token);

            if (!result.IsValid)
                return result.ToErrorsList();

            var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber).Value;
            var fullNameResult = FullName.Create(command.FullName.Name,
                command.FullName.SecondName,
                command.FullName.FamilyName).Value;
            var emailResult = Email.Create(command.Email).Value;
            var descriptionResult = Description.Create(command.Description).Value;
            var experienceResult = Experience.Create(command.Experience).Value;

            var volunteerResult = await _repository.GetByPhoneNumber(phoneNumberResult);

            if (volunteerResult.IsSuccess)
                return Errors.General.Conflict().ToErrorsList();

            var volunteer = new Volunteer(
                Guid.NewGuid(),
                fullNameResult,
                emailResult,
                descriptionResult,
                experienceResult,
                phoneNumberResult);

            await _repository.Add(volunteer, token);
            await _unitOfWork.SaveChanges(token);

            _logger.LogInformation("Volunteer {fullNmaeResult} with id {volunteerId} has been created", fullNameResult, volunteer.Id);

            return volunteer.Id;
        }
    }
}
