using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Commands.Close
{
    public class CloseDiscussionCommandValidator : AbstractValidator<CloseDiscussionCommand>
    {
        public CloseDiscussionCommandValidator()
        {
            RuleFor(c => c.DiscussionId)
                .NotEmpty().WithError(Errors.General.ValueIsInvalid("DiscussionId"));
        }
    }
}
