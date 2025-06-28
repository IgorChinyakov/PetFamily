using PetFamily.Discussions.Domain.ValueObjects.Discussion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Domain.ValueObjects.Message
{
    public class MessageId
    {
        private MessageId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static MessageId Create(Guid value) => new(value);

        public static MessageId New() => new(Guid.NewGuid());

        public static MessageId Empty() => new(Guid.Empty);
    }
}
