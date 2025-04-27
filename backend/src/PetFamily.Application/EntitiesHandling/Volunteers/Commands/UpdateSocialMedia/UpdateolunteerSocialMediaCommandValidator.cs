using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateSocialMedia
{
    public class UpdateolunteerSocialMediaCommandValidator : AbstractValidator<UpdateVolunteerSocialMediaCommand>
    {
        public UpdateolunteerSocialMediaCommandValidator()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("VolunteerId"));

            RuleForEach(u => u.SocialMedia)
                .MustBeValueObject(sm => SocialMedia.Create(sm.Title, sm.Link));
        }
    }
}