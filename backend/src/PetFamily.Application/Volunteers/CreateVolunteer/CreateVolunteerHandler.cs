using CSharpFunctionalExtensions;
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

        public CreateVolunteerHandler(IVolunteerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Guid, Error>> Handle(CreateVolunteerRequest request, CancellationToken token = default)
        {
            var phoneNumber = request.PhoneNumber;
            var phoneNumberResult = PhoneNumber.Create(phoneNumber);
            if (phoneNumberResult.IsFailure)
                return phoneNumberResult.Error;

            var volunteerResult = await _repository.GetByPhoneNumber(phoneNumberResult.Value);
            if (volunteerResult.IsSuccess)
                return Errors.General.Conflict("Volunteer");

            var fullNameDto = request.FullName;
            var fullNameResult = FullName.Create(fullNameDto.Name, fullNameDto.SecondName, fullNameDto.FamilyName);
            if (fullNameResult.IsFailure)
                return fullNameResult.Error;

            var email = request.Email;
            var emailResult = Email.Create(email);
            if (emailResult.IsFailure)
                return emailResult.Error;

            var description = request.Description;
            var descriptionResult = Description.Create(description);
            if (descriptionResult.IsFailure)
                return descriptionResult.Error;

            var experience = request.Experience;
            var experienceResult = Experience.Create(experience);
            if (experienceResult.IsFailure)
                return experienceResult.Error;

            var detailsDtos = request.DetailsList;
            List<Details> detailsList = new List<Details>();
            foreach(var dto in detailsDtos)
            {
                var details = Details.Create(dto.Title, dto.Description);
                if(details.IsFailure)
                    return details.Error;
                detailsList.Add(details.Value);
            }

            var socialMediaDtos = request.SocialMediaList;
            List<SocialMedia> socialMediaList = new List<SocialMedia>();
            foreach (var dto in socialMediaDtos)
            {
                var socialMedia = SocialMedia.Create(dto.Title, dto.Link);
                if (socialMedia.IsFailure)
                    return socialMedia.Error;
                socialMediaList.Add(socialMedia.Value);
            }

            var volunteer = new Volunteer(fullNameResult.Value,
                emailResult.Value,
                descriptionResult.Value,
                experienceResult.Value,
                phoneNumberResult.Value,
                detailsList,
                socialMediaList);

            var volunteerid = await _repository.Add(volunteer, token);

            return volunteerid;
        }
    }
}
