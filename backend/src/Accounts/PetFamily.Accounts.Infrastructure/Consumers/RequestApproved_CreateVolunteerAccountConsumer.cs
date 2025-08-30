using MassTransit;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.VolunteerRequests.Contracts.Messaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Infrastructure.Consumers
{
    public class RequestApproved_CreateVolunteerAccountConsumer : IConsumer<RequestApprovedEvent>
    {
        private readonly IAccountsContract _accountsContract;

        public RequestApproved_CreateVolunteerAccountConsumer(IAccountsContract accountsContract)
        {
            _accountsContract = accountsContract;
        }

        public async Task Consume(ConsumeContext<RequestApprovedEvent> context)
        {
            var createVolunteerAccountResult = await _accountsContract.CreateVolunteerAccount(
                new CreateVolunteerAccountRequest(context.Message.UserId));
            if (createVolunteerAccountResult.IsFailure)
                throw new Exception("Failed to create account");
        }
    }
}
