using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateDetails;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateSocialMedia;
using PetFamily.Core.Abstractions;
using PetFamily.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests.Volunteers
{
    public class UpdateVolunteerSocialMediaTests : TestsBase
    {
        private readonly ICommandHandler<Guid, UpdateVolunteerSocialMediaCommand> _sut;

        public UpdateVolunteerSocialMediaTests(IntegrationTestsWebFactory factory) : base(factory)
        {
            _sut = Scope.ServiceProvider
                .GetRequiredService<ICommandHandler<Guid, UpdateVolunteerSocialMediaCommand>>();
        }

        [Fact]
        public async Task UpdateDatails_should_be_success()
        {
            //Arrange
            var cancellationToken = new CancellationTokenSource().Token;

            var volunteerId = await SeedVolunteer();
            var command = Fixture.CreateUpdateVolunteerSocialMediaCommand(volunteerId);

            //Act
            var result = await _sut.Handle(command, cancellationToken);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            var volunteer = await ReadDbContext
                .Volunteers.FirstOrDefaultAsync(v => v.Id == command.Id, cancellationToken);

            volunteer!.SocialMedia.Should().BeEquivalentTo(command.SocialMedia);
        }
    }
}
