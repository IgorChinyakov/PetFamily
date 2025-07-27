using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Commands.Create
{
    public class CreateDiscussionCommandValidator : 
        AbstractValidator<CreateDiscussionCommand>
    {
        public CreateDiscussionCommandValidator()
        {
            RuleFor(c => c.RelationId)
                .NotEmpty().WithError(Errors.General.ValueIsInvalid("RequestId"));

            RuleForEach(c => c.UserIds)
                .NotEmpty().WithError(Errors.General.ValueIsInvalid("UserId"));
        }
    }
}
