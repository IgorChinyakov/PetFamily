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
        public const string VOLUNTEER = nameof(VOLUNTEER);

        public Guid Id { get; set; }

        public int Experience { get; set; } = 0;

        public List<Details> Details { get; set; } = [];

        public Guid UserId { get; set; }

        public User User { get; set; } = default!;

        //ef core
        public VolunteerAccount()
        {
        }

        public VolunteerAccount(User user)
        {
            User = user;
        }

        public void UpdateDetails(List<Details> details)
        {
            Details = details;
        }
    }
}
