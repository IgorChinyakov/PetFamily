using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Commands.AddMessage
{
    public record AddMessageToDiscussionCommand(Guid DiscussionId, Guid UserId, string Text) : ICommand;
}
