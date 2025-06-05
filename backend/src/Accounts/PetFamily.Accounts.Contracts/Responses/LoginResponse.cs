using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Contracts.Responses
{
    public record LoginResponse(string AccessToken, Guid RefreshToken );
}
