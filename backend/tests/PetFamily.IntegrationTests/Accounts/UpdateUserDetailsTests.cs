using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Features.UpdateDetails;
using PetFamily.Accounts.Application.Features.UpdateSocialMedia;
using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Accounts
{
    public class UpdateUserDetailsTests : TestsBase
    {
        private readonly ICommandHandler<Guid, UpdateUserDetailsCommand> _sut;

        public UpdateUserDetailsTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdateUserDetailsCommand>>();
        }

        [Fact]
        public async Task UpdateDatails_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var userId = await SeedVolunteerUser();
            var command = Fixture.CreateUpdateUserDetailsCommand(userId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var user = await UserManager
                .FindByIdAsync(command.Id.ToString());

            user.Should().NotBeNull();

            var volunteer = await VolunteerAccountManager.FindAccountByUserId(user.Id);

            volunteer!.Details.Should().BeEquivalentTo(command.Details);
        }
    }
}
