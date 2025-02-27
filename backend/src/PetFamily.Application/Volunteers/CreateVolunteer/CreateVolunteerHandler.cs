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
            var fullNameDto = request.FullName;
            var fullNameResult = FullName.Create(fullNameDto.Name, fullNameDto.SecondName, fullNameDto.FamilyName);
            if (fullNameResult.IsFailure)
                return fullNameResult.Error;

            var email = request.Email;
            var emailResult = Email.Create(email);
            if(emailResult.IsFailure)
                return emailResult.Error;

            var description = request.Description;
            var descriptionResult = Description.Create(description);
            if (descriptionResult.IsFailure)
                return descriptionResult.Error;

            var experience = request.Experience;
            var experienceResult = Experience.Create(experience);
            if (experienceResult.IsFailure)
                return experienceResult.Error;

            var detailsDto = request.Details;
            var detailsResult = Details.Create(detailsDto.Title, detailsDto.Description);
            if (detailsResult.IsFailure)
                return detailsResult.Error;

            var phoneNumber = request.PhoneNumber;
            var phoneNumberResult = PhoneNumber.Create(phoneNumber);
            if (detailsResult.IsFailure)
                return detailsResult.Error;

            var socialMediaDtos = request.SocialMediaList;
            List<SocialMedia> socialMediaList = [];
            foreach (var socialMediaDto in socialMediaDtos)
            {
                var socialMediaResult = SocialMedia.Create(socialMediaDto.Title, socialMediaDto.Link);
                if (socialMediaResult.IsFailure)
                    return socialMediaResult.Error;
                socialMediaList.Add(socialMediaResult.Value);
            }

            var volunteer = new Volunteer(fullNameResult.Value, 
                emailResult.Value, 
                descriptionResult.Value, 
                experienceResult.Value,
                detailsResult.Value, 
                phoneNumberResult.Value, 
                socialMediaList);

            var volunteerid = await _repository.Add(volunteer, token);

            return volunteerid;
        }
    }
}
