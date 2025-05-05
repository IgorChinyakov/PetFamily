using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.SharedVO;
using PetFamily.Volunteers.Domain.VolunteersVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo
{
    public class UpdateVolunteerMainInfoCommandValidator : AbstractValidator<UpdateVolunteerMainInfoCommand>
    {
        public UpdateVolunteerMainInfoCommandValidator()
        {
            RuleFor(u => u.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("VolunteersId"));

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
