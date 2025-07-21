using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Commands.Reject
{
    public class RejectRequestCommandValidator : 
        AbstractValidator<RejectRequestCommand>
    {
        public RejectRequestCommandValidator()
        {
            RuleFor(r => r.RequestId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("RequestId"));

            RuleFor(r => r.AdminId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("AdminId"));
        }
    }
}
