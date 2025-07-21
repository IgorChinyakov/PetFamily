using CSharpFunctionalExtensions;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects
{
    public class RequestId : ValueObject, IComparable<RequestId>
    {
        public Guid Value { get; }

        private RequestId(Guid value)
        {
            Value = value;
        }

        public static RequestId NewRequestId() => new(Guid.NewGuid());

        public static RequestId Empty() => new(Guid.Empty);

        public static RequestId Create(Guid id) => new(id);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public int CompareTo(RequestId? other)
        {
            if (other == null)
                throw new Exception();

            return Value.CompareTo(other.Value);
        }
    }
}