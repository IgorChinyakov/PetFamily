using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequests.Application.Features.Commands.Create;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.VolunteerRequests
{
    public class CreateRequestTests : TestsBase
    {
        private readonly ICommandHandler<Guid, CreateRequestCommand> _sut;

        public CreateRequestTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateRequestCommand>>();
        }

        [Fact]
        public async Task CreateRequest_with_existing_request_for_user_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            await SeedVolunteerRequest(userId);
            var command = Fixture.CreateCreateRequestCommand(userId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var count = await VolunteerRequestsReadDbContext.RequestDtos.CountAsync();

            count.Should().Be(1);
        }

        [Fact]
        public async Task CreateRequest_with_rejected_in_past_7_days_request_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            await SeedRejectedVolunteerRequest(userId);
            var command = Fixture.CreateCreateRequestCommand(userId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();

            var count = await VolunteerRequestsReadDbContext.RequestDtos.CountAsync();

            count.Should().Be(1);
        }

        [Fact]
        public async Task CreateRequest_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var command = Fixture.CreateCreateRequestCommand(userId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var requestsCount = await VolunteerRequestsReadDbContext.RequestDtos.CountAsync();

            requestsCount.Should().Be(1);
        }
    }
}
