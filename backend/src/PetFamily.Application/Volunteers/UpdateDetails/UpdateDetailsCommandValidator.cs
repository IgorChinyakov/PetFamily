using FluentValidation;
using PetFamily.Application.Volunteers.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.SharedVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.UpdateDetails
{
    public class UpdateDetailsCommandValidator : AbstractValidator<UpdateDetailsCommand>
    {
        public UpdateDetailsCommandValidator()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(u => u.Details)
                .MustBeValueObject(d => Details.Create(d.Title, d.Description));
        }
    }
}
