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

        public int Experience { get; set; }

        public List<Details> Details { get; set; } = [];

        public Guid UserId { get; set; }

        public User User { get; set; } = default!;

        //ef core
        public VolunteerAccount()
        {
        }

        public VolunteerAccount(User user, int experience)
        {
            //Id = Guid.NewGuid();
            User = user;
            Experience = experience;
        }

        public void UpdateDetails(List<Details> details)
        {
            Details = details;
        }
    }
}
