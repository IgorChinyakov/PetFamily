using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.AccountsFeatures.CreateVolunteerAccount
{
    public class CreateVolunteerAccountCommandValidator
        : AbstractValidator<CreateVolunteerAccountCommand>
    {
        public CreateVolunteerAccountCommandValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("Userid"));
        }
    }
}
