using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateDetails;

namespace PetFamily.IntegrationTests.Volunteers
{
    public class UpdateVolunteerDetailsTests : TestsBase
    {
        private readonly ICommandHandler<Guid, UpdateVolunteerDetailsCommand> _sut;

        public UpdateVolunteerDetailsTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdateVolunteerDetailsCommand>>();
        }

        [Fact]
        public async Task UpdateDatails_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var command = Fixture.CreateUpdateVolunteerDetailsCommand(volunteerId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await VolunteersReadDbContext
                .Volunteers.FirstOrDefaultAsync(v => v.Id == command.VolunteerId, cancellationToken);

            volunteer!.Details.Should().BeEquivalentTo(command.Details);
        }
    }
}
