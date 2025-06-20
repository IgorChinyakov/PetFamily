using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects
{
    public record RequestStatus
    {
        public Status Value { get; }

        //ef core
        private RequestStatus()
        {
        }

        private RequestStatus(Status status)
        {
            Value = status;
        }

        public static Result<RequestStatus> Create(Status status) => new RequestStatus(status); 

        public enum Status
        {
            Submitted,
            Rejected,
            RevisionRequired,
            Approved,
            OnReview
        }
    }
}
