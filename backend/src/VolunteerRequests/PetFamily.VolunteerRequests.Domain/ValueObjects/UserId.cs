using CSharpFunctionalExtensions;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects
{
    public class UserId : ValueObject
    {
        public Guid Value { get; }

        private UserId(Guid value)
        {
            Value = value;
        }

        public static UserId NewUserId() => new(Guid.NewGuid());

        public static UserId Empty() => new(Guid.Empty);

        public static UserId Create(Guid id) => new(id);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}