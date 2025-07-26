using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequests.Application.Features.Commands.SendForRevision;
using PetFamily.VolunteerRequests.Application.Features.Commands.TakeOnReview;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.VolunteerRequests
{
    public class SendRequestForRevisionTests : TestsBase
    {
        private readonly ICommandHandler<SendRequestForRevisionCommand> _sut;

        public SendRequestForRevisionTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<SendRequestForRevisionCommand>>();
        }

        [Fact]
        public async Task SendRequestForRevision_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedOnReviewVolunteerRequest(userId, adminId);
            var command = Fixture.CreateSendRequestForRevisionCommand(requestId, adminId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            var request = await VolunteerRequestsReadDbContext.RequestDtos.FirstOrDefaultAsync();
            request.Should().NotBeNull();
            request.RejectionComment.Should().NotBeNull();
            request.Status.Should().Be(RequestStatusDto.RevisionRequired);
        }

        [Fact]
        public async Task SendRequestForRevision_with_invalid_admin_id_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedOnReviewVolunteerRequest(userId, adminId);
            var command = Fixture.CreateSendRequestForRevisionCommand(requestId, Guid.NewGuid());

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();
            var request = await VolunteerRequestsReadDbContext.RequestDtos.FirstOrDefaultAsync();
            request.Should().NotBeNull();
            request.RejectionComment.Should().BeNull();
            request.Status.Should().Be(RequestStatusDto.OnReview);
        }

        [Fact]
        public async Task SendRequestForRevision_with_not_on_review_status_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var command = Fixture.CreateSendRequestForRevisionCommand(requestId, adminId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();
            var request = await VolunteerRequestsReadDbContext.RequestDtos.FirstOrDefaultAsync();
            request.Should().NotBeNull();
            request.RejectionComment.Should().BeNull();
        }
    }
}
