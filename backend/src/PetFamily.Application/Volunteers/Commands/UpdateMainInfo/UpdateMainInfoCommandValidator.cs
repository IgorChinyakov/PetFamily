using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfo
{
    public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
    {
        public UpdateMainInfoCommandValidator()
        {
            RuleFor(u => u.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(u => u.FullName)
                .MustBeValueObject(fn => FullName.Create(
                    fn.Name, fn.SecondName, fn.FamilyName))
                .When(u => u.FullName is not null);

            RuleFor(u => u.Email).MustBeValueObject(Email.Create);
            RuleFor(u => u.Experience).MustBeValueObject(Experience.Create);
            RuleFor(u => u.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
            RuleFor(u => u.Description).MustBeValueObject(Description.Create);
        }
    }
}
