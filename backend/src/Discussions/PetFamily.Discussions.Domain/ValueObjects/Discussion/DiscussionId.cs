using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Domain.ValueObjects.Discussion
{
    public class DiscussionId : ValueObject, IComparable<DiscussionId>
    {
        private DiscussionId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static DiscussionId Create(Guid value) => new(value);

        public static DiscussionId New() => new(Guid.NewGuid());

        public static DiscussionId Empty() => new(Guid.Empty);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public int CompareTo(DiscussionId? obj)
        {
            if (obj == null)
                throw new Exception();

            return Value.CompareTo(obj);
        }
    }
}
