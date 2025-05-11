using PetFamily.Accounts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Authorization
{
    public interface ITokenProvider
    {
        string GenerateAccessToken(User user, CancellationToken cancellationToken = default);
    }
}
