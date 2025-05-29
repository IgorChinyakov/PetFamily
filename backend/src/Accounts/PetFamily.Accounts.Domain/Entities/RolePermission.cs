using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.Entities
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }

        public Guid PermissionId {  get; set; }
        
        public Role Role { get; set; } = default!;

        public Permission Permission { get; set; } = default!;
    }
}
