using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequests.Application.Features.Commands.Create;
using PetFamily.VolunteerRequests.Application.Features.Commands.Update;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.VolunteerRequests
{
    public class UpdateRequestTests : TestsBase
    {
        private readonly ICommandHandler<UpdateRequestCommand> _sut;

        public UpdateRequestTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<UpdateRequestCommand>>();
        }

        [Fact]
        public async Task UpdateRequest_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedRevisionRequiredVolunteerRequest(userId, adminId);
            var command = Fixture.CreateUpdateRequestCommand(requestId, userId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            var request = await VolunteerRequestsReadDbContext.RequestDtos.FirstOrDefaultAsync();
            request.Should().NotBeNull();
            request.Status.Should().Be(RequestStatusDto.OnReview);
        }

        [Fact]
        public async Task UpdateRequest_with_invalid_user_id_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedRevisionRequiredVolunteerRequest(userId, adminId);
            var command = Fixture.CreateUpdateRequestCommand(requestId, Guid.NewGuid());

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var request = await VolunteerRequestsReadDbContext.RequestDtos.FirstOrDefaultAsync();
            request.Should().NotBeNull();
            request.Status.Should().Be(RequestStatusDto.RevisionRequired);
        }

        [Fact]
        public async Task UpdateRequest_with_invalid_status_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var command = Fixture.CreateUpdateRequestCommand(requestId, Guid.NewGuid());

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var request = await VolunteerRequestsReadDbContext.RequestDtos.FirstOrDefaultAsync();
            request.Should().NotBeNull();
            request.Status.Should().Be(RequestStatusDto.Submitted);
        }
    }
}
