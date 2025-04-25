using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.EntitiesHandling;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Delete;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Volunteers
{
    public class UpdateVolunteerDetailsTests : VolunteersTestsBase
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

            var volunteer = await ReadDbContext
                .Volunteers.FirstOrDefaultAsync(v => v.Id == command.VolunteerId, cancellationToken);

            volunteer!.Details.Should().BeEquivalentTo(command.Details);
        }
    }
}
