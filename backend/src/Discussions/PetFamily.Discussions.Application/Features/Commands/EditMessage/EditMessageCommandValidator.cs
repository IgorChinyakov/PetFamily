using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Domain.ValueObjects.Message;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Commands.EditMessage
{
    public class EditMessageCommandValidator : AbstractValidator<EditMessageCommand>
    {
        public EditMessageCommandValidator()
        {
            RuleFor(c => c.DiscussionId)
                .NotEmpty().WithError(Errors.General.ValueIsInvalid("DiscussionId"));

            RuleFor(c => c.MessageId)
                .NotEmpty().WithError(Errors.General.ValueIsInvalid("MessageId"));

            RuleFor(c => c.EditedMessage).MustBeValueObject(Text.Create);
        }
    }
}
