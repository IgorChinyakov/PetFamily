using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Outbox
{
    public sealed class OutboxMessage
    {
        public Guid Id { get; set; }

        public required string Type { get; set; } = string.Empty;

        public required string Payload { get; set; } = string.Empty;

        public required DateTime OccuredOnUtc { get; set; }

        public DateTime? ProcessedOnUtc { get; set; }

        public string? Error { get; set; }
    }
}
