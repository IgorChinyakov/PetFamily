using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Commands.Update
{
    public class UpdateRequestCommandValidator
        : AbstractValidator<UpdateRequestCommand>
    {
        public UpdateRequestCommandValidator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("UserId"));

            RuleFor(c => c.UpdatedInformation)
                .MustBeValueObject(VolunteerInformation.Create);
        }
    }
}
