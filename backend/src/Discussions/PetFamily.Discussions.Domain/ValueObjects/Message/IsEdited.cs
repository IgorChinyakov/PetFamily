using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Domain.ValueObjects.Message
{
    public class IsEdited
    {
        private IsEdited(bool value)
        {
            Value = value;
        }

        public bool Value { get; }

        public static IsEdited Create(bool value) 
            => new(value);
    }
}
