using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Features.UpdateSocialMedia;
using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Accounts
{
    public class UpdateUserSocialMediaTests : TestsBase
    {
        private readonly ICommandHandler<Guid, UpdateUserSocialMediaCommand> _sut;

        public UpdateUserSocialMediaTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdateUserSocialMediaCommand>>();
        }

        [Fact]
        public async Task UpdateSocialMedia_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var userId = await SeedParticipantUser();
            var command = Fixture.CreateUpdateUserSocialMediaCommand(userId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await UserManager
                .FindByIdAsync(command.Id.ToString());

            volunteer!.SocialMedia.Should().BeEquivalentTo(command.SocialMedia);
        }
    }
}
