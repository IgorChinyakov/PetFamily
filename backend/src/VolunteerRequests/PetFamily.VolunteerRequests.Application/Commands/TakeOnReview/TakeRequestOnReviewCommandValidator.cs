using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Commands.TakeOnReview
{
    public class TakeRequestOnReviewCommandValidator : 
        AbstractValidator<TakeRequestOnReviewCommand>
    {
        public TakeRequestOnReviewCommandValidator()
        {
            RuleFor(c => c.AdminId)
                .NotEmpty().WithError(Errors.General.ValueIsInvalid("AdminId"));

            RuleFor(c => c.RequestId)
                .NotEmpty().WithError(Errors.General.ValueIsInvalid("RequestId"));
        }
    }
}
