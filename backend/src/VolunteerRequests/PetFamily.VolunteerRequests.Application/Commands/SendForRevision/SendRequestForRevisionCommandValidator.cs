using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Commands.SendForRevision
{
    public class SendRequestForRevisionCommandValidator : 
        AbstractValidator<SendRequestForRevisionCommand>
    {
        public SendRequestForRevisionCommandValidator()
        {
            RuleFor(s => s.AdminId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("AdminId"));
        }
    }
}
