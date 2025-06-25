namespace PetFamily.VolunteerRequests.Domain.ValueObjects
{
    public record UserId
    {
        public Guid Value { get; }

        private UserId(Guid value)
        {
            Value = value;
        }

        public static UserId NewUserId() => new(Guid.NewGuid());

        public static UserId Empty() => new(Guid.Empty);

        public static UserId Create(Guid id) => new(id);
    }
}