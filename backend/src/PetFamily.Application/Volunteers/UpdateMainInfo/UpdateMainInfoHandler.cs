using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Volunteers.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
{
    public class UpdateMainInfoHandler
    {
        private readonly IVolunteerRepository _repository;
        private readonly IValidator<UpdateMainInfoCommand> _validator;

        public UpdateMainInfoHandler(
            IVolunteerRepository volunteerRepository, 
            IValidator<UpdateMainInfoCommand> validator)
        {
            _repository = volunteerRepository;
            _validator = validator;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            UpdateMainInfoCommand command,
            CancellationToken token = default)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var volunteerGetByIdResult = await _repository.GetById(command.Id);

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

            await _repository.Save(volunteerGetByIdResult.Value, token);

            return volunteerGetByIdResult.Value.Id;
        }
    }
}
