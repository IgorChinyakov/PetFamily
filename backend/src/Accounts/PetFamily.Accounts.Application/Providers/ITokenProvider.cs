using PetFamily.Accounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Providers
{
    public interface ITokenProvider
    {
        string GenerateAccessToken(User user, CancellationToken cancellationToken = default);
    }
}
