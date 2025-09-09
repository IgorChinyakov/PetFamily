using MassTransit;
using MassTransit.Internals.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Discussions.Contracts;
using PetFamily.VolunteerRequests.Contracts;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;
using Polly;
using Polly.Retry;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Outbox
{
    public class ProcessRequestsOutboxMessagesService
    {
        private readonly VolunteerRequestsWriteDbContext _dbContext;
        private readonly ILogger<ProcessRequestsOutboxMessagesService> _logger;
        private readonly IPublishEndpoint _publisher;

        public ProcessRequestsOutboxMessagesService(
            VolunteerRequestsWriteDbContext dbContext,
            ILogger<ProcessRequestsOutboxMessagesService> logger,
            IPublishEndpoint publisher)
        {
            _dbContext = dbContext;
            _logger = logger;
            _publisher = publisher;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var messages = await _dbContext.OutboxMessages
                .OrderBy(o => o.OccuredOnUtc)
                .Where(o => o.ProcessedOnUtc == null)
                .Take(20)
                .ToListAsync();

            if (!messages.Any())
                return;

            var pipeline = new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Exponential,
                    Delay = TimeSpan.FromSeconds(1),
                    ShouldHandle = new PredicateBuilder().Handle<Exception>(ex => ex is not NullReferenceException),
                    OnRetry = retryArguments =>
                    {
                        _logger.LogCritical(retryArguments.Outcome.Exception, "Current attempt: {attemptNumber}", retryArguments.AttemptNumber);

                        return ValueTask.CompletedTask;
                    }
                })
                .Build();

            var outboxMessages = messages.Select(message => ProcessMessage(message, pipeline, cancellationToken));
            await Task.WhenAll(outboxMessages);

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save changes to database.");
            }
        }

        private async Task ProcessMessage(
            OutboxMessage outboxMessage, 
            ResiliencePipeline pipeline, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await pipeline.ExecuteAsync(async token =>
                {
                    var messageType = RequestsAssemblyReference.Assembly.GetType(outboxMessage.Type)
                                      ?? throw new NullReferenceException("Message type not found");

                    var deserializedMessage = JsonSerializer.Deserialize(outboxMessage.Payload, messageType)
                                              ?? throw new NullReferenceException("Message payload not found");

                    await _publisher.Publish(deserializedMessage, messageType, token);

                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                outboxMessage.Error = ex.Message;
                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                _logger.LogError(ex, "Failed to process Mesage ID: {MessageId}", outboxMessage.Id);
            }
        }
    }
}
