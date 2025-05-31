using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        private List<Role> _roles = [];

        private User(string userName, string email, FullName fullName, Role role)
        {
            FullName = fullName;
            UserName = userName;
            Email = email;
            _roles = [role];
        }

        //ef core
        public User()
        {
        }

        public IReadOnlyList<Role> Roles => _roles;

        public string PathToPhoto { get; set; } = string.Empty;

        public FullName FullName { get; set; } = default!;

        public List<SocialMedia> SocialMedia { get; set; } = [];

        public AdminAccount? AdminAccount { get; set; }

        public ParticipantAccount? ParticipantAccount { get; set; }

        public VolunteerAccount? VolunteerAccount { get; set; }

        public static Result<User, Error> CreateAdmin(
            string userName, 
            string email, 
            FullName fullName, 
            Role role)
        {
            if (role.Name != AdminAccount.ADMIN)
                return Errors.General.ValueIsInvalid("role");

            return new User(userName, email, fullName, role);
        }

        public static Result<User, Error> CreateParticipant(
            string userName,
            string email,
            FullName fullName,
            Role role)
        {
            if (role.Name != ParticipantAccount.PARTICIPANT)
                return Errors.General.ValueIsInvalid("role");

            return new User(userName, email, fullName, role);
        }
    }
}
