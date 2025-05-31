using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.Entities
{
    public class ParticipantAccount
    {
        public const string PARTICIPANT = nameof(PARTICIPANT);

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = default!;

        public List<Guid> FavoritePets { get; set; } = [];

        //ef core
        public ParticipantAccount()
        {
        }

        public ParticipantAccount(User user)
        {
            Id = Guid.NewGuid();
            User = user;
        }
    }
}
