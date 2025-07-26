using AutoFixture;
using PetFamily.Accounts.Application.Features.UpdateDetails;
using PetFamily.Accounts.Application.Features.UpdateSocialMedia;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Core.Options;
using PetFamily.Specieses.Application.Breeds.Commands.Create;
using PetFamily.Specieses.Application.Breeds.Commands.Delete;
using PetFamily.Specieses.Application.Specieses.Commands.Create;
using PetFamily.Specieses.Application.Specieses.Commands.Delete;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Specieses.Domain.ValueObjects;
using PetFamily.VolunteerRequests.Application.Features.Commands.Approve;
using PetFamily.VolunteerRequests.Application.Features.Commands.Create;
using PetFamily.VolunteerRequests.Application.Features.Commands.Reject;
using PetFamily.VolunteerRequests.Application.Features.Commands.SendForRevision;
using PetFamily.VolunteerRequests.Application.Features.Commands.TakeOnReview;
using PetFamily.VolunteerRequests.Application.Features.Commands.Update;
using PetFamily.VolunteerRequests.Domain.Entities;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using PetFamily.Volunteers.Application.Pets.Commands.Create;
using PetFamily.Volunteers.Application.Pets.Commands.Delete;
using PetFamily.Volunteers.Application.Pets.Commands.Move;
using PetFamily.Volunteers.Application.Pets.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Application.Pets.Commands.UpdateStatus;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;
using PetFamily.Volunteers.Application.Volunteers.Commands.Delete;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.PetsVO;
using PetFamily.Volunteers.Domain.SharedVO;
using PetFamily.Volunteers.Domain.VolunteersVO;
using static PetFamily.Volunteers.Domain.PetsVO.PetStatus;

namespace PetFamily.IntegrationTests
{
    public static class FixtureExtensions
    {
        public static CreateRequestCommand CreateCreateRequestCommand(
            this IFixture fixture, Guid userId)
        {
            return fixture.Build<CreateRequestCommand>()
                .With(c => c.UserId, userId)
                .Create();
        }

        public static SendRequestForRevisionCommand CreateSendRequestForRevisionCommand(
            this IFixture fixture, Guid requestId, Guid adminId)
        {
            return fixture.Build<SendRequestForRevisionCommand>()
                .With(c => c.RequestId, requestId)
                .With(c => c.AdminId, adminId)
                .Create();
        }

        public static RejectRequestCommand CreateRejectRequestCommand(
            this IFixture fixture, Guid requestId, Guid adminId)
        {
            return fixture.Build<RejectRequestCommand>()
                .With(c => c.RequestId, requestId)
                .With(c => c.AdminId, adminId)
                .Create();
        }

        public static ApproveRequestCommand CreateApproveRequestCommand(
            this IFixture fixture, Guid requestId, Guid adminId)
        {
            return fixture.Build<ApproveRequestCommand>()
                .With(c => c.RequestId, requestId)
                .With(c => c.AdminId, adminId)
                .Create();
        }

        public static UpdateRequestCommand CreateUpdateRequestCommand(
           this IFixture fixture, Guid requestId, Guid userId)
        {
            return fixture.Build<UpdateRequestCommand>()
                .With(c => c.RequestId, requestId)
                .With(c => c.UserId, userId)
                .Create();
        }

        public static TakeRequestOnReviewCommand CreateTakeRequestOnReviewCommand(
            this IFixture fixture, Guid requestId, Guid adminId)
        {
            return fixture.Build<TakeRequestOnReviewCommand>()
                .With(c => c.RequestId, requestId)
                .With(c => c.AdminId, adminId)
                .Create();
        }

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

        public static UpdateUserSocialMediaCommand CreateUpdateUserSocialMediaCommand(
            this IFixture fixture,
            Guid userId)
        {
            return fixture.Build<UpdateUserSocialMediaCommand>()
                .With(d => d.Id, userId)
                .Create();
        }

        public static UpdateUserDetailsCommand CreateUpdateUserDetailsCommand(
            this IFixture fixture,
            Guid userId)
        {
            return fixture.Build<UpdateUserDetailsCommand>()
                .With(d => d.Id, userId)
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
                PhoneNumber.Create("89103454545").Value);
        }

        public static KeyValuePair<string, User> CreateParticipantUser(
            this IFixture fixture,
            Role role)
        {
            return new KeyValuePair<string, User>(
                "participant123@A",
                User.CreateParticipant(
                fixture.Create<string>(),
                "oojdngjndjg@gmail.com",
                new PetFamily.Accounts.Domain.ValueObjects.FullName
                {
                    Name = fixture.Create<string>(),
                    SecondName = fixture.Create<string>(),
                    FamilyName = fixture.Create<string>()
                },
                role).Value);
        }

        public static KeyValuePair<string, User> CreateAdminUser(
            this IFixture fixture,
            Role role)
        {
            return new KeyValuePair<string, User>(
                "admin123@A",
                User.CreateAdmin(
                fixture.Create<string>(),
                "oojdngjndjg@gmail.com",
                new PetFamily.Accounts.Domain.ValueObjects.FullName
                {
                    Name = fixture.Create<string>(),
                    SecondName = fixture.Create<string>(),
                    FamilyName = fixture.Create<string>()
                },
                role).Value);
        }

        public static KeyValuePair<string, User> CreateVolunteerUser(
            this IFixture fixture,
            Role role)
        {
            return new KeyValuePair<string, User>(
                "volunteer123@A",
                User.CreateVolunteer(
                fixture.Create<string>(),
                "qqqqwwrwrrr@gmail.com",
                new PetFamily.Accounts.Domain.ValueObjects.FullName
                {
                    Name = fixture.Create<string>(),
                    SecondName = fixture.Create<string>(),
                    FamilyName = fixture.Create<string>()
                },
                role).Value);
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
                PetFamily.Volunteers.Domain.PetsVO.CreationDate.Create(DateTime.UtcNow.AddDays(-new Random().Next(1, 365))).Value,
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

        public static VolunteerRequest CreateVolunteerRequest(
            this IFixture fixture,
            Guid userId)
        {
            return VolunteerRequest.Create(
                UserId.Create(userId), 
                VolunteerInformation.Create(fixture.Create<string>()).Value).Value;
        }
    }
}
