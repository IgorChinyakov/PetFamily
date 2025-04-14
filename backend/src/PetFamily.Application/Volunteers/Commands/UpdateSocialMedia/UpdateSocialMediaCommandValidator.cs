using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Application.Volunteers.Commands.UpdateSocialMedia
{
    public class UpdateSocialMediaCommandValidator : AbstractValidator<UpdateSocialMediaCommand>
    {
        public UpdateSocialMediaCommandValidator()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(u => u.SocialMedia)
                .MustBeValueObject(sm => SocialMedia.Create(sm.Title, sm.Link));
        }
    }
}