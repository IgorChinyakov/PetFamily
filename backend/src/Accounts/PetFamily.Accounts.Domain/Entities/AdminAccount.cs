using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.Entities
{
    public class AdminAccount
    {
        public const string ADMIN = nameof(ADMIN);

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = default!;

        //ef core
        public AdminAccount()
        {
        }

        public AdminAccount(User user)
        {
            //Id = Guid.NewGuid();
            User = user;
        }
    }
}
