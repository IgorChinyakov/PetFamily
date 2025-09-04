using PetFamily.Core.Abstractions.Database;
using PetFamily.Discussions.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Infrastructure.Outbox
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly DiscussionsWriteDbContext _context;

        public OutboxRepository(DiscussionsWriteDbContext context)
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
