using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.Commands.UpdateDetails;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfo
{
    public class UpdateMainInfoHandler : ICommandHandler<Guid, UpdateMainInfoCommand>
    {
        private readonly IVolunteerRepository _repository;
        private readonly IValidator<UpdateMainInfoCommand> _validator;
        private readonly ILogger<UpdateMainInfoHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMainInfoHandler(
            IVolunteerRepository volunteerRepository,
            IValidator<UpdateMainInfoCommand> validator,
            ILogger<UpdateMainInfoHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _repository = volunteerRepository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            UpdateMainInfoCommand command,
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
                return Errors.General.Conflict().ToErrorsList();

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
