using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Presentation
{
    public class Permissions
    {
        public class VolunteerRequest
        {
            public const string GET_BY_USER_ID = "volunteer.requests.get.by.user.id";
            public const string GET = "volunteer.requests.get";
            public const string CREATE = "volunteer.requests.create";
            public const string UPDATE_STATUS = "volunteer.requests.update.status";
            public const string UPDATE = "volunteer.requests.update";
        }
    }
}
