using PetFamily.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.Commands.Delete
{
    public record DeleteVolunteerCommand(Guid Id, DeletionOptions Options) : ICommand;
}
