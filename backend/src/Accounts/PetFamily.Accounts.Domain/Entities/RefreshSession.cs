using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.Entities
{
    public class RefreshSession
    {
        public Guid Id { get; set; }

        public Guid RefreshToken { get; set; }

        public Guid Jti {  get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresIn { get; set; }
    }
}
