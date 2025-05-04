using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.VolunteersVO;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialMedia
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