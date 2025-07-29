using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Features.Commands.AddMessage;
using PetFamily.Discussions.Application.Features.Commands.Create;
using PetFamily.VolunteerRequests.Application.Features.Commands.Create;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Discussions
{
    public class CreateDiscussionTests : TestsBase
    {
        private readonly ICommandHandler<Guid, CreateDiscussionCommand> _sut;

        public CreateDiscussionTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateDiscussionCommand>>();
        }

        [Fact]
        public async Task CreateDiscussion_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var command = Fixture.CreateCreateDiscussionCommand(requestId, [userId, adminId]);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            var discussion = await DiscussionsReadDbContext.Discussions.FirstOrDefaultAsync();

            discussion.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateDiscussion_with_existing_discussion_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var command = Fixture.CreateCreateDiscussionCommand(requestId, [userId, adminId]);
            await SeedDiscussion(userId, adminId, requestId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task CreateDiscussion_with_invalid_users_count_should_be_failure()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            await SeedAccounts();

            var userId = await SeedParticipantUser();
            var adminId = await SeedAdminUser();
            var requestId = await SeedVolunteerRequest(userId);
            var command = Fixture.CreateCreateDiscussionCommand(requestId, [userId, adminId, Guid.NewGuid()]);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
