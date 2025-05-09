using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.SharedVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateDetails
{
    public class UpdateVolunteerDetailsCommandValidator : AbstractValidator<UpdateVolunteerDetailsCommand>
    {
        public UpdateVolunteerDetailsCommandValidator()
        {
            RuleFor(u => u.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("VolunteerId"));

            RuleForEach(u => u.Details)
                .MustBeValueObject(d => Details.Create(d.Title, d.Description));
        }
    }
}
