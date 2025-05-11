using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Contracts.Requests
{
    public record LoginUserRequest(string Email, string Password);
}
