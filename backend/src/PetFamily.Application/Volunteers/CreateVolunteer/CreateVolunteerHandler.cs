using CSharpFunctionalExtensions;
using FluentValidation;
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
        private IValidator<CreateVolunteerRequest> _validator;

        public CreateVolunteerHandler(IVolunteerRepository repository,
            IValidator<CreateVolunteerRequest> validator)
        {
            _repository = repository;
            _validator = validator; 
        }

        public async Task<Result<Guid, Error>> Handle(CreateVolunteerRequest request, CancellationToken token = default)
        {
            var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber).Value;
            var fullNameResult = FullName.Create(request.FullName.Name, 
                request.FullName.SecondName, 
                request.FullName.FamilyName).Value;
            var emailResult = Email.Create(request.Email).Value;
            var descriptionResult = Description.Create(request.Description).Value;
            var experienceResult = Experience.Create(request.Experience).Value;

            var detailsList = request.DetailsList.Select(d => Details.Create(d.Title, d.Description).Value).ToList();
            var socialMediaList = request.SocialMediaList.Select(d => SocialMedia.Create(d.Title, d.Link).Value).ToList();
            
            var volunteerResult = await _repository.GetByPhoneNumber(phoneNumberResult);
            if (volunteerResult.IsSuccess)
                return Errors.General.Conflict("Volunteer");

            var volunteer = new Volunteer(fullNameResult,
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
