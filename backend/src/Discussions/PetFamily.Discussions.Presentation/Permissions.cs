using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Presentation
{
    public class Permissions
    {
        public class Discussions
        {
            public const string UPDATE = "discussions.update";
            public const string GET = "discussions.get";
        }

        public class Messages
        {
            public const string DELETE = "messages.delete";
            public const string UPDATE = "messages.update";
            public const string CREATE = "messages.create";
        }
    }
}
