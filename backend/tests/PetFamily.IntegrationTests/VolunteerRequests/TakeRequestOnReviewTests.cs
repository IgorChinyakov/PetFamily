using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequests.Application.Features.Commands.Create;
using PetFamily.VolunteerRequests.Application.Features.Commands.TakeOnReview;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.VolunteerRequests
{
    public class TakeRequestOnReviewTests : TestsBase
    {
        private readonly ICommandHandler<TakeRequestOnReviewCommand> _sut;

        public TakeRequestOnReviewTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<TakeRequestOnReviewCommand>>();
        }

        [Fact]
        public async Task TakeRequestOnReview_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var command = Fixture.CreateTakeRequestOnReviewCommand(requestId, adminId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            var request = await VolunteerRequestsReadDbContext.RequestDtos.FirstOrDefaultAsync();
            request.Should().NotBeNull();
            request.DiscussionId.Should().NotBe(Guid.Empty);
            request.AdminId.Should().NotBe(Guid.Empty);
            request.Status.Should().Be(RequestStatusDto.OnReview);
        }

        [Fact]
        public async Task TakeRequestOnReview_with_invalid_request_id_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var command = Fixture.CreateTakeRequestOnReviewCommand(Guid.NewGuid(), adminId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();
            var request = await VolunteerRequestsReadDbContext.RequestDtos.FirstOrDefaultAsync();
            request.Should().NotBeNull();
            request.DiscussionId.Should().Be(Guid.Empty);
            request.AdminId.Should().Be(Guid.Empty);
            request.Status.Should().Be(RequestStatusDto.Submitted);
        }
    }
}
