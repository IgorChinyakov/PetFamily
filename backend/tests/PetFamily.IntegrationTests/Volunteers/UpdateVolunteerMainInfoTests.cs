using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;

namespace PetFamily.IntegrationTests.Volunteers
{
    public class UpdateVolunteerMainInfoTests : TestsBase
    {
        private readonly ICommandHandler<Guid, UpdateVolunteerMainInfoCommand> _sut;

        public UpdateVolunteerMainInfoTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdateVolunteerMainInfoCommand>>();
        }

        [Fact]
        public async Task UpdateDatails_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var command = Fixture.CreateUpdateVolunteerMainInfoCommand(volunteerId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await VolunteersReadDbContext
                .Volunteers.FirstOrDefaultAsync(v => v.Id == command.VolunteerId, cancellationToken);

            volunteer!.PhoneNumber.Should().Be(command.PhoneNumber);
            volunteer!.Description.Should().Be(command.Description);
            volunteer!.Email.Should().Be(command.Email);
            volunteer!.FullName.Should().BeEquivalentTo(command.FullName);
        }
    }
}
