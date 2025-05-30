using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Presentation
{
    public static class Permissions
    {
        public static class Volunteer
        {
            public const string CREATE = "volunteers.create";
            public const string GET = "volunteers.get";
            public const string DELETE = "volunteers.delete";
            public const string UPDATE = "volunteers.update";
        }

        public static class Pet
        {
            public const string CREATE = "volunteers.create.pet";
            public const string GET = "volunteers.get.pet";
            public const string DELETE = "volunteers.delete.pet";
            public const string UPDATE = "volunteers.update.pet";
        }
    }
}
