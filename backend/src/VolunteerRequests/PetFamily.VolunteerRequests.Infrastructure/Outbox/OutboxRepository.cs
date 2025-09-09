using PetFamily.Core.Abstractions.Database;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Outbox
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly VolunteerRequestsWriteDbContext _context;

        public OutboxRepository(VolunteerRequestsWriteDbContext context)
        {
            _context = context;
        }

        public async Task Add<T>(T message, CancellationToken cancellationToken)
        {
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccuredOnUtc = DateTime.Now,
                Type = typeof(T).FullName!,
                Payload = JsonSerializer.Serialize(message)
            };

            await _context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        }
    }
}
