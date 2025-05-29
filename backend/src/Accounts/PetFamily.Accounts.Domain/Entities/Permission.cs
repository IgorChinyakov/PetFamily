using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public List<Role> Roles { get; set; } = [];
    }
}
