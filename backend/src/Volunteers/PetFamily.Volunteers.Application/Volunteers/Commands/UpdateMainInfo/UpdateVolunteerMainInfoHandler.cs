using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Domain.SharedVO;
using PetFamily.Volunteers.Domain.VolunteersVO;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo
{
    public class UpdateVolunteerMainInfoHandler :
        ICommandHandler<Guid, UpdateVolunteerMainInfoCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<UpdateVolunteerMainInfoCommand> _validator;
        private readonly ILogger<UpdateVolunteerMainInfoHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVolunteerMainInfoHandler(
            IVolunteersRepository volunteerRepository,
            IValidator<UpdateVolunteerMainInfoCommand> validator,
            ILogger<UpdateVolunteerMainInfoHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Volunteers)] IUnitOfWork unitOfWork)
        {
            _repository = volunteerRepository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            UpdateVolunteerMainInfoCommand command,
            CancellationToken token = default)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var volunteerGetByIdResult = await _repository.GetById(command.VolunteerId);

            if (volunteerGetByIdResult.IsFailure)
                return Errors.General.ValueIsInvalid("VolunteerId").ToErrorsList();

            var fullName = FullName.Create(
                command.FullName.Name,
                command.FullName.SecondName,
                command.FullName.FamilyName).Value;
            var email = Email.Create(command.Email).Value;
            var experience = Experience.Create(command.Experience).Value;
            var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
            var description = Description.Create(command.Description).Value;

            var volunteerGetByPhoneResult = await _repository.GetByPhoneNumber(phoneNumber);
            if (volunteerGetByPhoneResult.IsSuccess)
                return Errors.General.Conflict("volunteer").ToErrorsList();

            volunteerGetByIdResult.Value.UpdateMainInfo(
                fullName,
                description,
                phoneNumber,
                email,
                experience);

            await _unitOfWork.SaveChanges(token);

            _logger.LogInformation("Volunteer's main info has been updated. Volunteer Id: {volunteerId}", volunteerGetByIdResult.Value.Id);

            return volunteerGetByIdResult.Value.Id;
        }
    }
}
