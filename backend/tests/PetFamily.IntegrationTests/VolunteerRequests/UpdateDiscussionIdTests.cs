using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequests.Application.Features.Commands.TakeOnReview;
using PetFamily.VolunteerRequests.Application.Features.Commands.UpdateDiscussionId;
using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.VolunteerRequests
{
    public class UpdateDiscussionIdTests : TestsBase
    {
        private readonly ICommandHandler<UpdateDiscussionIdCommand> _sut;

        public UpdateDiscussionIdTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<UpdateDiscussionIdCommand>>();
        }

        [Fact]
        public async Task UpdateDiscussionId_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var command = new UpdateDiscussionIdCommand(requestId, Guid.NewGuid());

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            var request = await VolunteerRequestsReadDbContext.RequestDtos.FirstOrDefaultAsync();
            request!.DiscussionId.Should().NotBe(Guid.Empty);
        }
    }
}
