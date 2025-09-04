using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Outbox
{
    [DisallowConcurrentExecution]
    public class ProcessRequestsOutboxMessagesJob : IJob
    {
        private readonly ProcessRequestsOutboxMessagesService _service;

        public ProcessRequestsOutboxMessagesJob(ProcessRequestsOutboxMessagesService service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _service.Execute(context.CancellationToken);
        }
    }
}
