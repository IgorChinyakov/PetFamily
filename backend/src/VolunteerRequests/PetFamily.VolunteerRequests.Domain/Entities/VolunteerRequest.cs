using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Domain.Entities
{
    public class VolunteerRequest : Entity<RequestId>
    {
        //ef core
        private VolunteerRequest()
        {
        }

        public AdminId? AdminId { get; private set; }

        public DiscussionId? DiscussionId { get; private set; }

        public UserId UserId { get; private set; }

        public RequestStatus RequestStatus { get; private set; }

        public CreationDate CreationDate { get; private set; }

        public VolunteerInformation VolunteerInformation { get; private set; }

        public RejectionComment? RejectionComment { get; private set; }

        public RejectedRequest? RejectedRequest { get; private set; }

        public VolunteerRequest(
            RequestId requestId,
            UserId userId,
            RequestStatus requestStatus,
            CreationDate createdAt,
            VolunteerInformation volunteerInformation) : base(requestId)
        {
            Id = requestId;
            UserId = userId;
            RequestStatus = requestStatus;
            CreationDate = createdAt;
            VolunteerInformation = volunteerInformation;
        }

        public static Result<VolunteerRequest, Error> Create(UserId userId, VolunteerInformation volunteerInformation)
        {
            var id = RequestId.NewRequestId();

            return new VolunteerRequest(
                id,
                userId,
                RequestStatus.Create(RequestStatus.Status.Submitted).Value,
                CreationDate.Create(DateTime.UtcNow).Value,
                volunteerInformation);
        }

        public UnitResult<Error> SendForRevision(RejectionComment rejectionComment)
        {
            if (RequestStatus.Value != RequestStatus.Status.OnReview)
                return Error.Failure(
                    "invalid.request.status", 
                    "Request status should be OnReview", 
                    "RequestStatus");

            RequestStatus = RequestStatus.Create(RequestStatus.Status.RevisionRequired).Value;

            RejectionComment = rejectionComment;

            return Result.Success<Error>();
        }

        public UnitResult<Error> TakeOnReview(AdminId adminId, DiscussionId discussionId)
        {
            if (RequestStatus.Value != RequestStatus.Status.Submitted)
                return Error.Failure(
                    "invalid.request.status",
                    "Request status should be Submitted",
                    "RequestStatus");

            RequestStatus = RequestStatus.Create(RequestStatus.Status.OnReview).Value;
            AdminId = adminId;
            DiscussionId = discussionId;

            return Result.Success<Error>();
        }

        public void Reject()
        {
            RequestStatus = RequestStatus.Create(RequestStatus.Status.Rejected).Value;
            var rejectedRequest = new RejectedRequest(Id, RejectionDate.Create(DateTime.UtcNow).Value);
            RejectedRequest = rejectedRequest;
        }

        public void Approve()
         => RequestStatus = RequestStatus.Create(RequestStatus.Status.Approved).Value;

        public void UpdateVolunteerInformation(VolunteerInformation volunteerInformation)
            => VolunteerInformation = volunteerInformation;
    }
}
