using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Features.Commands.Close;
using PetFamily.Discussions.Application.Features.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Discussions
{
    public class CloseDiscussionTests : TestsBase
    {
        private readonly ICommandHandler<CloseDiscussionCommand> _sut;

        public CloseDiscussionTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<CloseDiscussionCommand>>();
        }

        [Fact]
        public async Task CloseDiscussion_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var discussion = await SeedDiscussion(userId, adminId, requestId);
            var command = new CloseDiscussionCommand(discussion.Id.Value);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task CloseDiscussion_with_invalid_discussion_id_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var discussion = await SeedDiscussion(userId, adminId, requestId);
            var command = new CloseDiscussionCommand(Guid.NewGuid());

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
