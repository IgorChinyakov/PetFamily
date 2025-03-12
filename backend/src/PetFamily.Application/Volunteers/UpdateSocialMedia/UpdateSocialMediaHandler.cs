using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Volunteers.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.UpdateSocialMedia
{
    public class UpdateSocialMediaHandler
    {
        private readonly IVolunteerRepository _repository;
        private readonly IValidator<UpdateSocialMediaCommand> _validator;

        public UpdateSocialMediaHandler(
            IVolunteerRepository repository, 
            IValidator<UpdateSocialMediaCommand> validator)
        {
            _repository = repository;
            _validator = validator; 
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            UpdateSocialMediaCommand command, 
            CancellationToken token = default)
        {
            var validationResult = await _validator.ValidateAsync(command, token);

            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var volunteerResult = await _repository.GetById(command.Id);
            if (volunteerResult.IsFailure)
                return Errors.General.ValueIsInvalid("VolunteerId").ToErrorsList();

            var socialMedia = command.SocialMedia.Select(sm => SocialMedia.Create(sm.Title, sm.Link).Value); 
            volunteerResult.Value.UpdateSocialMediaList(socialMedia);
            await _repository.Save(volunteerResult.Value, token);

            return volunteerResult.Value.Id;
        }
    }
}
