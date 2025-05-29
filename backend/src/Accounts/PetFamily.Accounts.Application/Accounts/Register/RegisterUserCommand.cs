using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Accounts.Register
{
    public record RegisterUserCommand(
        string Email, 
        string Password, 
        string UserName, 
        string FirstName,
        string SecondName,
        string LastName) : ICommand;
}
