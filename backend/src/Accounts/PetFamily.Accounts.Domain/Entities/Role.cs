using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public List<Permission> Permissions { get; set; } = [];
    }
}
