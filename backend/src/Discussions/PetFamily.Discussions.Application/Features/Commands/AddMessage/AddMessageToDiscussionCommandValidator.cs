using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Domain.ValueObjects.Message;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Commands.AddMessage
{
    public class AddMessageToDiscussionCommandValidator : 
        AbstractValidator<AddMessageToDiscussionCommand>
    {
        public AddMessageToDiscussionCommandValidator()
        {
            RuleFor(a => a.DiscussionId)
                .NotEmpty().WithError(Errors.General.ValueIsInvalid("DiscussionId"));

            RuleFor(a => a.UserId)
                .NotEmpty().WithError(Errors.General.ValueIsInvalid("userId"));

            RuleFor(a => a.Text).MustBeValueObject(Text.Create);
        }
    }
}
