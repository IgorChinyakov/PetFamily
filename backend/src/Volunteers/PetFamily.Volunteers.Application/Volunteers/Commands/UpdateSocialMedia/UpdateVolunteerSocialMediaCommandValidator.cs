using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.VolunteersVO;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialMedia
{
    public class UpdateVolunteerSocialMediaCommandValidator : 
        AbstractValidator<UpdateVolunteerSocialMediaCommand>
    {
        public UpdateVolunteerSocialMediaCommandValidator()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("VolunteerId"));

            RuleForEach(u => u.SocialMedia)
                .MustBeValueObject(sm => SocialMedia.Create(sm.Title, sm.Link));
        }
    }
}