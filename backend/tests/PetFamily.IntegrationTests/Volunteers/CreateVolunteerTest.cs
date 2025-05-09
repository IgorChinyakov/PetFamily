using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;

namespace PetFamily.IntegrationTests.Volunteers
{
    public class CreateVolunteerTest :
        TestsBase
    {
        private readonly ICommandHandler<Guid, CreateVolunteerCommand> _sut;

        public CreateVolunteerTest(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
        }

        [Fact]
        public async Task CreateVolunteer_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;
            var command = Fixture.CreateCreateVolunteerCommand();

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteers = await VolunteersReadDbContext.Volunteers.ToListAsync();

            volunteers.Should().NotBeNull();
            volunteers.Should().HaveCount(1);
        }
    }
}