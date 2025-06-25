namespace PetFamily.VolunteerRequests.Domain.ValueObjects
{
    public class RequestId
    {
        public Guid Value { get; }

        private RequestId(Guid value)
        {
            this.Value = value;
        }

        public static RequestId NewRequestId() => new(Guid.NewGuid());

        public static RequestId Empty() => new(Guid.Empty);

        public static RequestId Create(Guid id) => new(id);
    }
}