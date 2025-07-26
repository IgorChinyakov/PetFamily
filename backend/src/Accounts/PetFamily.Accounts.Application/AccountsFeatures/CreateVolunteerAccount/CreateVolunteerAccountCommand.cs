using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.AccountsFeatures.CreateVolunteerAccount
{
    public record CreateVolunteerAccountCommand(Guid UserId) : ICommand;
}
