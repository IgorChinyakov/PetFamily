using FluentValidation;
using PetFamily.Application.Volunteers.Validation;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
    {
        public CreateVolunteerCommandValidator()
        {
            RuleFor(c => c.Email).MustBeValueObject(Email.Create);
            RuleFor(c => c.Experience).MustBeValueObject(Experience.Create);
            RuleFor(c => c.Description).MustBeValueObject(Description.Create);
            RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

            RuleFor(c => new { c.FullName.Name, c.FullName.SecondName, c.FullName.FamilyName })
                .MustBeValueObject(fn => FullName.Create(fn.Name, fn.SecondName, fn.FamilyName));

            RuleForEach(c => c.DetailsList)
                .MustBeValueObject(d => SocialMedia.Create(d.Title, d.Description));

            RuleForEach(c => c.SocialMediaList)
                .MustBeValueObject(sm => SocialMedia.Create(sm.Title, sm.Link));
        }
    }
}
