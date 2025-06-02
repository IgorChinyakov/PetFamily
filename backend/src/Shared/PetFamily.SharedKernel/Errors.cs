using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.SharedKernel
{
    public class Errors
    {
        public class General
        {
            public static Error ValueIsInvalid(string? name = null)
            {
                var label = name ?? "value";
                return Error.Validation("value.is.invalid", $"{label} is invalid", name ?? label);
            }

            public static Error NotFound(Guid? id = null)
            {
                var forId = id == null ? "" : $" for id '{id}'";
                return Error.NotFound("record.not.found", $"record not found{forId}", null);
            }

            public static Error Conflict(string? name = null)
            {
                var label = name == null ? "" : name + " ";
                return Error.Conflict("already.exists", $"{label}already exists", null);
            }

            public static Error Conflict()
            {
                return Error.Conflict("connected.entities.exist", $"Entity can't be removed due to connected entities existence", null);
            }
        }

        public class Authorization
        {
            public static Error InvalidCredentials()
            {
                return Error.Validation("invalid.credentials", "Credentials are invalid");
            }

            public static Error AccessDenied(Guid? id = null)
            {
                var label = id == null ? "User " : $"User with {id} ";
                return Error.AccessDenied("inappropriate.role", $"{label}has inappropriate role");
            }
        }
    }
}
