using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.EntitiesHandling;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Create;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Delete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Volunteers
{
    public class DeleteVolunteerTests
        : VolunteersTestsBase
    {
        private readonly ICommandHandler<Guid, DeleteVolunteerCommand> _sut;

        public DeleteVolunteerTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, DeleteVolunteerCommand>>();
        }

        [Fact]
        public async Task HardDeleteVolunteer_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var command = Fixture.CreateDeleteVolunteerCommand(volunteerId, DeletionOptions.Hard);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteers = await ReadDbContext
                .Volunteers.ToListAsync();

            volunteers.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task SoftDeleteVolunteer_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var command = Fixture.CreateDeleteVolunteerCommand(volunteerId, DeletionOptions.Soft);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await WriteDbContext
                .Volunteers.FirstOrDefaultAsync();

            volunteer!.IsDeleted.Should().BeTrue();
        }
    }
}
