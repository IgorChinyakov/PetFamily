using PetFamily.Accounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Presentation
{
    public class Permissions
    {
        public const string UPDATE = "accounts.update";

        public class VolunteerAccount
        {
            public const string UPDATE = "accounts.volunteer.update";
        }
    }
}
