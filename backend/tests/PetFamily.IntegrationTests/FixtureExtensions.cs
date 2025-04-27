using AutoFixture;
using PetFamily.Api.Controllers;
using PetFamily.Application.EntitiesHandling;
using PetFamily.Application.EntitiesHandling.Breeds.Commands.Create;
using PetFamily.Application.EntitiesHandling.Breeds.Commands.Delete;
using PetFamily.Application.EntitiesHandling.Pets.Commands.Create;
using PetFamily.Application.EntitiesHandling.Pets.Commands.Move;
using PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateMainInfo;
using PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateStatus;
using PetFamily.Application.EntitiesHandling.Pets.Commands.UploadPhotos;
using PetFamily.Application.EntitiesHandling.Specieses.Commands.Create;
using PetFamily.Application.EntitiesHandling.Specieses.Commands.Delete;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Create;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Delete;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateDetails;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateSocialMedia;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.ValueObjects;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.PetsVO;
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

        public static CreateSpeciesCommand CreateCreateSpeciesCommand(
           this IFixture fixture)
        {
            return fixture.Build<CreateSpeciesCommand>()
                .With(c => c.Name, "name")
                .Create();
        }

        public static DeleteSpeciesCommand CreateDeleteSpeciesCommand(
           this IFixture fixture,
           Guid speciesId)
        {
            return fixture.Build<DeleteSpeciesCommand>()
                .With(c => c.Id, speciesId)
                .Create();
        }

        public static MovePetCommand CreateMovePetCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid petId,
            int positionToMove)
        {
            return fixture.Build<MovePetCommand>()
                .With(c => c.VolunteerId, volunteerId)
                .With(c => c.PetId, petId)
                .With(c => c.PositionToMove, positionToMove)
                .Create();
        }

        public static CreatePetCommand CreateCreatePetCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid speciesId,
            Guid breedId)
        {
            return fixture.Build<CreatePetCommand>()
                .With(c => c.VolunteerId, volunteerId)
                .With(c => c.SpeciesId, speciesId)
                .With(c => c.BreedId, breedId)
                .With(c => c.NickName, "Nickname")
                .With(c => c.Birthday, DateTime.UtcNow.AddDays(-new Random().Next(1, 365)))
                .Create();
        }

        public static CreateBreedCommand CreateCreateBreedCommand(
           this IFixture fixture,
           Guid speciesId)
        {
            return fixture.Build<CreateBreedCommand>()
                .With(c => c.SpeciesId, speciesId)
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

        public static DeletePetCommand CreateDeletePetCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid petId,
            DeletionOptions options)
        {
            return fixture.Build<DeletePetCommand>()
                .With(d => d.VolunteerId, volunteerId)
                .With(d => d.PetId, petId)
                .With(d => d.Options, options)
                .Create();
        }

        public static DeleteBreedCommand CreateDeleteBreedCommand(
            this IFixture fixture,
            Guid speciesId,
            Guid breedId)
        {
            return fixture.Build<DeleteBreedCommand>()
                .With(d => d.SpeciesId, speciesId)
                .With(d => d.BreedId, breedId)
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

        public static UpdatePetMainInfoCommand CreateUpdatePetMainInfoCommand(
           this IFixture fixture,
           Guid volunteerId,
           Guid petId,
           Guid speciesId,
           Guid breedId)
        {
            return fixture.Build<UpdatePetMainInfoCommand>()
                .With(c => c.NickName, "Nickname")
                .With(c => c.PhoneNumber, "89113454545")
                .With(c => c.SpeciesId, speciesId)
                .With(c => c.BreedId, breedId)
                .With(c => c.PetId, petId)
                .With(c => c.VolunteerId, volunteerId)
                .With(c => c.Birthday, DateTime.UtcNow.AddDays(-new Random().Next(1, 365)))
                .With(c => c.CreationDate, DateTime.UtcNow.AddDays(-new Random().Next(1, 365)))
                .Create();
        }

        public static UpdatePetStatusCommand CreateUpdatePetStatusCommand(
           this IFixture fixture,
           Guid volunteerId,
           Guid petId)
        {
            return fixture.Build<UpdatePetStatusCommand>()
                .With(c => c.PetId, petId)
                .With(c => c.VolunteerId, volunteerId)
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

        public static Pet CreatePet(
            this IFixture fixture,
            Guid speciesId,
            Guid breedId)
        {
            return new Pet(
                Guid.Empty,
                NickName.Create("Nickname").Value,
                Description.Create(fixture.Create<string>()).Value,
                SpeciesId.Create(speciesId).Value,
                BreedId.Create(breedId).Value,
                Color.Create(fixture.Create<string>()).Value,
                IsSterilized.Create(fixture.Create<bool>()).Value,
                IsVaccinated.Create(fixture.Create<bool>()).Value,
                HealthInformation.Create(fixture.Create<string>()).Value,
                Address.Create(
                    fixture.Create<string>(),
                    fixture.Create<string>(),
                    fixture.Create<string>()).Value,
                Weight.Create(fixture.Create<int>()).Value,
                Height.Create(fixture.Create<int>()).Value,
                Birthday.Create(DateTime.UtcNow.AddDays(-new Random().Next(1, 365))).Value,
                CreationDate.Create(DateTime.UtcNow.AddDays(-new Random().Next(1, 365))).Value,
                PhoneNumber.Create("89103454545").Value,
                PetStatus.Create(fixture.Create<Status>()).Value,
                [Details.Create(fixture.Create<string>(), fixture.Create<string>()).Value]
                );
        }

        public static Species CreateSpecies(this IFixture fixture)
        {
            return new Species(
                Guid.NewGuid(),
                Name.Create("Name").Value);
        }

        public static Breed CreateBreed(this IFixture fixture)
        {
            return new Breed(
                Guid.Empty,
                Name.Create("Name").Value);
        }
    }
}
