using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Specieses.Presentation
{
    public static class Permissions
    {
        public static class Species
        {
            public const string CREATE = "species.create";
            public const string GET = "species.get";
            public const string DELETE = "species.delete";
            public const string UPDATE = "species.update";
        }

        public static class Breeds
        {
            public const string CREATE = "species.create.breed";
            public const string GET = "species.get.breed";
            public const string DELETE = "species.delete.breed";
            public const string UPDATE = "species.update.breed";
        }
    }
}
