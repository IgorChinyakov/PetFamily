using FluentValidation;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Features.UpdateDetails
{
    public class UpdateUserDetailsCommandValidator : 
        AbstractValidator<UpdateUserDetailsCommand>
    {
        public UpdateUserDetailsCommandValidator()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("VolunteerId"));

            RuleForEach(u => u.Details)
                .MustBeValueObject(d => Details.Create(d.Title, d.Description));
        }
    }
}
