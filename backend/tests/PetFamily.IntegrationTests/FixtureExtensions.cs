using AutoFixture;
using PetFamily.Application.EntitiesHandling;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Create;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Delete;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateDetails;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateSocialMedia;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.IntegrationTests
{
    public static class FixtureExtensions
    {
        public static CreateVolunteerCommand CreateCreateVolunteerCommand(
            this IFixture fixture)
        {
            return fixture.Build<CreateVolunteerCommand>()
                .With(c => c.PhoneNumber, "89103454545")
                .With(c => c.Email, "oojdngjndjg@gmail.com")
                .Create();
        }

        public static DeleteVolunteerCommand CreateDeleteVolunteerCommand(
            this IFixture fixture,
            Guid volunteerId,
            DeletionOptions options)
        {
            return fixture.Build<DeleteVolunteerCommand>()
                .With(d => d.Id, volunteerId)
                .With(d => d.Options, options)
                .Create();
        }

        public static UpdateVolunteerDetailsCommand CreateUpdateVolunteerDetailsCommand(
            this IFixture fixture,
            Guid volunteerId)
        {
            return fixture.Build<UpdateVolunteerDetailsCommand>()
                .With(d => d.VolunteerId, volunteerId)
                .Create();
        }

        public static UpdateVolunteerSocialMediaCommand CreateUpdateVolunteerSocialMediaCommand(
            this IFixture fixture,
            Guid volunteerId)
        {
            return fixture.Build<UpdateVolunteerSocialMediaCommand>()
                .With(d => d.Id, volunteerId)
                .Create();
        }

        public static UpdateVolunteerMainInfoCommand CreateUpdateVolunteerMainInfoCommand(
            this IFixture fixture,
            Guid volunteerId)
        {
            return fixture.Build<UpdateVolunteerMainInfoCommand>()
                .With(c => c.VolunteerId, volunteerId)
                .With(c => c.PhoneNumber, "89113454545")
                .With(c => c.Email, "ndjg@gmail.com")
                .Create();
        }

        public static Volunteer CreateVolunteer(this IFixture fixture)
        {
            return new Volunteer(
                Guid.NewGuid(),
                FullName.Create(
                    fixture.Create<string>(), 
                    fixture.Create<string>(), 
                    fixture.Create<string>()).Value,
                Email.Create("oojdngjndjg@gmail.com").Value,
                Description.Create(fixture.Create<string>()).Value,
                Experience.Create(fixture.Create<int>()).Value,
                PhoneNumber.Create("89103454545").Value,
                [Details.Create(fixture.Create<string>(), fixture.Create<string>()).Value],
                [SocialMedia.Create(fixture.Create<string>(), fixture.Create<string>()).Value]);
        }
    }
}
