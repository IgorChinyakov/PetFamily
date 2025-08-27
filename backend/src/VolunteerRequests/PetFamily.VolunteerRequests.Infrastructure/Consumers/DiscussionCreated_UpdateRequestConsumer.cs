using MassTransit;
using PetFamily.Discussions.Contracts.Messaging;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.VolunteerRequests.Contracts;
using PetFamily.VolunteerRequests.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Consumers
{
    public class DiscussionCreated_UpdateRequestConsumer : IConsumer<DiscussionCreatedEvent>
    {
        private readonly IVolunteerRequestsContract _volunteerRequestsContract;

        public DiscussionCreated_UpdateRequestConsumer(IVolunteerRequestsContract volunteerRequestsContract)
        {
            _volunteerRequestsContract = volunteerRequestsContract;
        }

        public async Task Consume(ConsumeContext<DiscussionCreatedEvent> context)
        {
            var result = await _volunteerRequestsContract.UpdateDiscussionId(
                context.Message.RelationId, 
                context.Message.DiscussionId);
            if (result.IsFailure)
                throw new Exception("Failed to update request");
        }
    }
}
