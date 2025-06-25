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

            // act
            var request = VolunteerRequest.Create(userId, "some information");

            // assert
            request.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Create_Request_With_Invalid_Information_Should_Return_Failure()
        {
            // arrange
            var userId = UserId.Create(Guid.NewGuid());

            // act
            var request = VolunteerRequest.Create(userId, "");

            // assert
            request.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void SendForRevision_Should_Return_Success()
        {
            // arrange
            var userId = UserId.Create(Guid.NewGuid());
            var request = VolunteerRequest.Create(userId, "some information");

            // act
            var sendForRevisionResult = request.Value.SendForRevision("rejection comment");

            // assert
            sendForRevisionResult.IsSuccess.Should().BeTrue();
            request.Value.RequestStatus.Value.Should().Be(RequestStatus.Status.RevisionRequired);
            request.Value.RejectionComment.Should().NotBeNull();
        }

        [Fact]
        public void SendForRevision_With_Invalid_Rejection_Comment_Should_Return_Failure()
        {
            // arrange
            var userId = UserId.Create(Guid.NewGuid());
            var request = VolunteerRequest.Create(userId, "some information");

            // act
            var sendForRevisionResult = request.Value.SendForRevision("");

            // assert
            sendForRevisionResult.IsSuccess.Should().BeFalse();
            request.Value.RequestStatus.Value.Should().Be(RequestStatus.Status.Submitted);
            request.Value.RejectionComment.Should().BeNull();
        }

        [Fact]
        public void TakeOnReview_Should_Change_Status_And_AdminId()
        {
            // arrange
            var userId = UserId.Create(Guid.NewGuid());
            var request = VolunteerRequest.Create(userId, "some information");
            var adminId = AdminId.Create(Guid.NewGuid());

            // act
            request.Value.TakeOnReview(adminId);

            // assert
            request.Value.RequestStatus.Value.Should().Be(RequestStatus.Status.OnReview);
            request.Value.AdminId.Should().Be(adminId);
        }

        [Fact]
        public void Reject_Should_Change_Status()
        {
            // arrange
            var userId = UserId.Create(Guid.NewGuid());
            var request = VolunteerRequest.Create(userId, "some information");
            var adminId = AdminId.Create(Guid.NewGuid());
            request.Value.TakeOnReview(adminId);

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
            var request = VolunteerRequest.Create(userId, "some information");
            var adminId = AdminId.Create(Guid.NewGuid());
            request.Value.TakeOnReview(adminId);

            // act
            request.Value.Approve();

            // assert
            request.Value.RequestStatus.Value.Should().Be(RequestStatus.Status.Approved);
        }
    }
}
