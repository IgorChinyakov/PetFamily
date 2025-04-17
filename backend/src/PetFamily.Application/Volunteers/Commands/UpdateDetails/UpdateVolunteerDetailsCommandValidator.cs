using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.SharedVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.Commands.UpdateDetails
{
    public class UpdateVolunteerDetailsCommandValidator : AbstractValidator<UpdateVolunteerDetailsCommand>
    {
        public UpdateVolunteerDetailsCommandValidator()
        {
            RuleFor(u => u.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(u => u.Details)
                .MustBeValueObject(d => Details.Create(d.Title, d.Description));
        }
    }
}
