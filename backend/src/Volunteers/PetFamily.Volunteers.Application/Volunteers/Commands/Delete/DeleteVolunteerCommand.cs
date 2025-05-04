using PetFamily.Core.Abstractions;
using PetFamily.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Delete
{
    public record DeleteVolunteerCommand(Guid Id, DeletionOptions Options) : ICommand;
}
