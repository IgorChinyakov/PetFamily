using PetFamily.Accounts.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.Entities
{
    public class VolunteerAccount
    {
        public Guid Id { get; set; }

        public int Experience { get; set; }

        public List<Details> Details { get; set; } = [];

        public Guid UserId { get; set; }

        public User User { get; set; } = default!;
    }
}
