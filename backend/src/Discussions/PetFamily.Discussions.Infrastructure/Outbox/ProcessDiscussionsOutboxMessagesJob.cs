using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Infrastructure.Outbox
{
    [DisallowConcurrentExecution]
    public class ProcessDiscussionsOutboxMessagesJob : IJob
    {
        private readonly ProcessDiscussionsOutboxMessagesService _service;

        public ProcessDiscussionsOutboxMessagesJob(ProcessDiscussionsOutboxMessagesService service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _service.Execute(context.CancellationToken);
        }
    }
}
