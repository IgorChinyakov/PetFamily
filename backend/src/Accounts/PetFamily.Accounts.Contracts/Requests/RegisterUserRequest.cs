using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Contracts.Requests
{
    public record RegisterUserRequest(string Email, string Password, string UserName);
}
