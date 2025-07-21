using FluentAssertions;
using PetFamily.VolunteerRequests.Domain.Entities;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.UnitTests
{
    public class VolunteerRequestTests
    {
        [Fact]
        public void Create_Request_Should_Return_Success()
        {
            // arrange
            var userId = UserId.Create(Guid.NewGuid());
            var information = VolunteerInformation.Create("some information").Value;

            // act
            var request = VolunteerRequest.Create(userId, information);

            // assert
            request.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void TakeOnReview_Should_Change_Status_AdminId_And_DiscussionId()
        {
            // arrange
            var userId = UserId.Create(Guid.NewGuid());
            var information = VolunteerInformation.Create("some information").Value;
            var request = VolunteerRequest.Create(userId, information);
            var adminId = AdminId.Create(Guid.NewGuid());
            var discussionId = DiscussionId.Create(Guid.NewGuid());

            // act
            request.Value.TakeOnReview(adminId, discussionId);

            // assert
            request.Value.RequestStatus.Value.Should().Be(RequestStatus.Status.OnReview);
            request.Value.AdminId.Should().Be(adminId);
            request.Value.DiscussionId.Should().Be(discussionId);
        }

        [Fact]
        public void SendForRevision_Should_Return_Success()
        {
            // arrange
            var userId = UserId.Create(Guid.NewGuid());
            var information = VolunteerInformation.Create("some information").Value;
            var rejectionComment = RejectionComment.Create("rejection comment").Value;
            var request = VolunteerRequest.Create(userId, information);
            var discussionId = DiscussionId.Create(Guid.NewGuid());
            var adminId = AdminId.Create(Guid.NewGuid());
            request.Value.TakeOnReview(adminId, discussionId);

            // act
            var sendForRevisionResult = request.Value.SendForRevision(rejectionComment);

            // assert
            sendForRevisionResult.IsSuccess.Should().BeTrue();
            request.Value.RequestStatus.Value.Should().Be(RequestStatus.Status.RevisionRequired);
            request.Value.RejectionComment.Should().NotBeNull();
        }

        [Fact]
        public void Reject_Should_Change_Status()
        {
            // arrange
            var userId = UserId.Create(Guid.NewGuid());
            var information = VolunteerInformation.Create("some information").Value;
            var request = VolunteerRequest.Create(userId, information);
            var adminId = AdminId.Create(Guid.NewGuid());
            var discussionId = DiscussionId.Create(Guid.NewGuid());
            request.Value.TakeOnReview(adminId, discussionId);

            // act
            request.Value.Reject();

            // assert
            request.Value.RequestStatus.Value.Should().Be(RequestStatus.Status.Rejected);
        }

        [Fact]
        public void Approve_Should_Should_Change_Status()
        {
            // arrange
            var userId = UserId.Create(Guid.NewGuid());
            var information = VolunteerInformation.Create("some information").Value;
            var request = VolunteerRequest.Create(userId, information);
            var adminId = AdminId.Create(Guid.NewGuid());
            var discussionId = DiscussionId.Create(Guid.NewGuid());
            request.Value.TakeOnReview(adminId, discussionId);

            // act
            request.Value.Approve();

            // assert
            request.Value.RequestStatus.Value.Should().Be(RequestStatus.Status.Approved);
        }
    }
}
