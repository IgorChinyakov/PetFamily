using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Commands.EditMessage
{
    public record EditMessageCommand(
        Guid DiscussionId, 
        Guid MessageId, 
        Guid UserId,
        string EditedMessage) : ICommand;
}
