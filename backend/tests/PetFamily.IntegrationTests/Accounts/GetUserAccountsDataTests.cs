using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Accounts.GetAccountsData;
using PetFamily.Accounts.Application.Features.UpdateDetails;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Infrastructure.Authorization.Managers;
using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Accounts
{
    public class GetUserAccountsDataTests : TestsBase
    {
        private readonly ICommandHandler<AccountsDataResponse, GetUserAccountsDataCommand> _sut;

        public GetUserAccountsDataTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<AccountsDataResponse, GetUserAccountsDataCommand>>();
        }

        [Fact]
        public async Task GetUserAccountsData_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var userId = await SeedParticipantUser();
            var command = new GetUserAccountsDataCommand(userId);
            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();

            var user = await UserManager
                .FindByIdAsync(command.Id.ToString());

            user.Should().NotBeNull();

            var participant = await ParticipantAccountManager.FindAccountByUserId(user.Id);

            participant.Should().NotBeNull();
            user.AdminAccount.Should().BeNull();
            user.VolunteerAccount.Should().BeNull();
            user.ParticipantAccount.Should().NotBeNull();
            user.ParticipantAccount.Id.Should().Be(participant.Id);
        }
    }
}
