using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Volunteers.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private IVolunteerRepository _repository;
        private IValidator<CreateVolunteerCommand> _validator;

        public CreateVolunteerHandler(IVolunteerRepository repository,
            IValidator<CreateVolunteerCommand> validator)
        {
            _repository = repository;
            _validator = validator; 
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

            var detailsList = command.DetailsList.Select(d => Details.Create(d.Title, d.Description).Value).ToList();
            var socialMediaList = command.SocialMediaList.Select(d => SocialMedia.Create(d.Title, d.Link).Value).ToList();
            
            var volunteerResult = await _repository.GetByPhoneNumber(phoneNumberResult);

            if (volunteerResult.IsSuccess)
                return Errors.General.Conflict("Volunteer").ToErrorsList();

            var volunteer = new Volunteer(
                fullNameResult,
                emailResult,
                descriptionResult,
                experienceResult,
                phoneNumberResult,
                detailsList,
                socialMediaList);

            var volunteerid = await _repository.Add(volunteer, token);

            return volunteerid;
        }
    }
}
