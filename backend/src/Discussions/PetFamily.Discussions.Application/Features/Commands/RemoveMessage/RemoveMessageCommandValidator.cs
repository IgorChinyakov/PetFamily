using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Commands.RemoveMessage
{
    public class RemoveMessageCommandValidator : AbstractValidator<RemoveMessageCommand>
    {
        public RemoveMessageCommandValidator()
        {
            RuleFor(c => c.MessageId).NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("MessageId"));

            RuleFor(c => c.DiscussionId).NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("DiscussionId"));
        }
    }
}
