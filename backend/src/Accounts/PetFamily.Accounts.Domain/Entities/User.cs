using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string PathToPhoto { get; set; } = string.Empty;

        public FullName FullName { get; set; } = default!;

        public List<SocialMedia> SocialMedia { get; set; } = [];

        public AdminAccount? AdminAccount { get; set; }

        public ParticipantAccount? ParticipantAccount { get; set; }

        public VolunteerAccount? VolunteerAccount { get; set; }
    }
}
