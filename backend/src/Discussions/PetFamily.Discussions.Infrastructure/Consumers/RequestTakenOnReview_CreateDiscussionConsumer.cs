using MassTransit;
using PetFamily.Discussions.Contracts;
using PetFamily.Discussions.Contracts.Messaging;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.VolunteerRequests.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Infrastructure.Consumers
{
    public class RequestTakenOnReview_CreateDiscussionConsumer : IConsumer<RequestTakenOnReviewEvent>
    {
        private readonly IDiscussionsContract _discussionsContract;
        private readonly IPublishEndpoint _publishEndpoint;

        public RequestTakenOnReview_CreateDiscussionConsumer(
            IDiscussionsContract discussionsContract, 
            IPublishEndpoint publishEndpoint)
        {
            _discussionsContract = discussionsContract;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<RequestTakenOnReviewEvent> context)
        {
            var request = new CreateDiscussionRequest(context.Message.RequestId, context.Message.UserIds);
            var discussionResult = await _discussionsContract.CreateDiscussion(request, context.CancellationToken);
            if (discussionResult.IsFailure)
                throw new Exception("Failed to create discussion");

            await _publishEndpoint.Publish(new DiscussionCreatedEvent(context.Message.RequestId, discussionResult.Value));
        }
    }
}
