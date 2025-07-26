using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequests.Application.Features.Commands.Approve;
using PetFamily.VolunteerRequests.Application.Features.Commands.Reject;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.VolunteerRequests
{
    public class ApproveRequestTests : TestsBase
    {
        private readonly ICommandHandler<ApproveRequestCommand> _sut;

        public ApproveRequestTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<ApproveRequestCommand>>();
        }

        [Fact]
        public async Task ApproveRequest_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedOnReviewVolunteerRequest(userId, adminId);
            var command = Fixture.CreateApproveRequestCommand(requestId, adminId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            var request = await VolunteerRequestsReadDbContext.RequestDtos.FirstOrDefaultAsync();
            request.Should().NotBeNull();
            request.Status.Should().Be(RequestStatusDto.Approved);
        }

        [Fact]
        public async Task RejectRequest_with_invalid_admin_id_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedOnReviewVolunteerRequest(userId, adminId);
            var command = Fixture.CreateApproveRequestCommand(requestId, Guid.NewGuid());

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();
            var request = await VolunteerRequestsReadDbContext.RequestDtos.FirstOrDefaultAsync();
            request.Should().NotBeNull();
            request.Status.Should().Be(RequestStatusDto.OnReview);
        }
    }
}
