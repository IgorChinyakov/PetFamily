using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects
{
    public class AdminId : ValueObject
    {
        public Guid Value { get; }

        private AdminId(Guid value)
        {
            Value = value;
        }

        public static AdminId NewAdminId() => new(Guid.NewGuid());

        public static AdminId Empty() => new(Guid.Empty);

        public static AdminId Create(Guid id) => new(id);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
