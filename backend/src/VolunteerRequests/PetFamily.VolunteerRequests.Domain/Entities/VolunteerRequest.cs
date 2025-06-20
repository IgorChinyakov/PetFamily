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
    public class VolunteerRequest
    {
        //ef core
        private VolunteerRequest()
        {
        }

        public RequestId Id { get; set; }

        public AdminId? AdminId { get; set; }

        public DiscussionId? DiscussionId { get; set; }

        public UserId UserId { get; set; }

        public RequestStatus RequestStatus { get; set; }

        public CreationDate CreationDate { get; set; }

        public VolunteerInformation VolunteerInformation { get; set; }

        public RejectionComment? RejectionComment { get; set; }

        public VolunteerRequest(
            RequestId requestId,
            UserId userId,
            RequestStatus requestStatus,
            CreationDate createdAt,
            VolunteerInformation volunteerInformation)
        {
            Id = requestId;
            UserId = userId;
            RequestStatus = requestStatus;
            CreationDate = createdAt;
            VolunteerInformation = volunteerInformation;
        }

        public static Result<VolunteerRequest, Error> Create(UserId userId, string volunteerInformation)
        {
            var informationResult = VolunteerInformation.Create(volunteerInformation);
            if (informationResult.IsFailure)
                return informationResult.Error;

            var id = RequestId.NewRequestId();

            return new VolunteerRequest(
                id,
                userId,
                RequestStatus.Create(RequestStatus.Status.Submitted).Value,
                CreationDate.Create(DateTime.UtcNow).Value,
                informationResult.Value);
        }

        public UnitResult<Error> SendForRevision(string rejectionComment)
        {
            var commentResult = RejectionComment.Create(rejectionComment);
            if (commentResult.IsFailure)
                return commentResult.Error;

            RequestStatus = RequestStatus.Create(RequestStatus.Status.RevisionRequired).Value;

            RejectionComment = commentResult.Value;

            return Result.Success<Error>();
        }

        public void TakeOnReview(AdminId adminId)
        {
            RequestStatus = RequestStatus.Create(RequestStatus.Status.OnReview).Value;
            AdminId = adminId;
        }

        public void Reject()
         => RequestStatus = RequestStatus.Create(RequestStatus.Status.Rejected).Value;

        public void Approve()
         => RequestStatus = RequestStatus.Create(RequestStatus.Status.Approved).Value;
    }
}
