using FluentAssertions;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Domain.UnitTests
{
    public class VolunteerTests
    {
        [Fact]
        public void Add_Pet_With_Empty_Pets_Return_Success_Result()
        {
            // arrange
            var volunteer = CreateVolunteer();
            var petToAdd = CreatePet();

            // act
            var result = volunteer.AddPet(petToAdd);

            // assert
            var addedPetResult = volunteer.GetPetById(petToAdd.Id);

            result.IsSuccess.Should().BeTrue();
            addedPetResult.IsSuccess.Should().BeTrue();
            addedPetResult.Value.Id.Should().Be(petToAdd.Id);
            addedPetResult.Value.Position.Should().Be(petToAdd.Position);
        }

        [Fact]
        public void Add_Pet_With_Other_Pets_Return_Success_Result()
        {
            // arrange
            const int petsCount = 5;
            var volunteer = CreateVolunteerWithPets(petsCount);

            var petToAdd = CreatePet();

            // act
            var result = volunteer.AddPet(petToAdd);

            // assert

            var addedPetResult = volunteer.GetPetById(petToAdd.Id);

            result.IsSuccess.Should().BeTrue();
            addedPetResult.IsSuccess.Should().BeTrue();
            addedPetResult.Value.Id.Should().Be(petToAdd.Id);
            addedPetResult.Value.Position.Should().Be(petToAdd.Position);
        }

        [Fact]
        public void Move_Pet_When_Current_And_New_Position_Same_Should_Not_Move()
        {
            // arrange
            const int petsCount = 6;
            var volunteer = CreateVolunteerWithPets(petsCount);

            var positionToMove = Position.Create(5).Value;

            var firstPet = volunteer.Pets[0];
            var secondPet = volunteer.Pets[1];
            var thirdPet = volunteer.Pets[2];
            var fourthPet = volunteer.Pets[3];
            var fifthPet = volunteer.Pets[4];
            var sixthPet = volunteer.Pets[5];

            // act
            var result = volunteer.MovePet(fifthPet, positionToMove);

            //  assert
            result.IsSuccess.Should().BeTrue();
            firstPet.Position.Value.Should().Be(1);
            secondPet.Position.Value.Should().Be(2);
            thirdPet.Position.Value.Should().Be(3);
            fourthPet.Position.Value.Should().Be(4);
            fifthPet.Position.Value.Should().Be(5);
            sixthPet.Position.Value.Should().Be(6);
        }

        [Fact]
        public void Move_Pet_Should_Be_Success_When_Pets_Count_One()
        {
            // arrange
            const int petsCount = 1;
            var volunteer = CreateVolunteerWithPets(petsCount);

            var positionToMove = Position.Create(1).Value;

            var firstPet = volunteer.Pets[0];

            // act
            var result = volunteer.MovePet(firstPet, positionToMove);

            //  assert
            result.IsSuccess.Should().BeTrue();
            firstPet.Position.Value.Should().Be(1);
        }

        [Fact]
        public void Move_Pet_Should_Move_Other_Pets_Forward_When_New_Position_Lower_Than_Current()
        {
            // arrange
            const int petsCount = 6;
            var volunteer = CreateVolunteerWithPets(petsCount);

            var positionToMove = Position.Create(2).Value;

            var firstPet = volunteer.Pets[0];
            var secondPet = volunteer.Pets[1];
            var thirdPet = volunteer.Pets[2];
            var fourthPet = volunteer.Pets[3];
            var fifthPet = volunteer.Pets[4];
            var sixthPet = volunteer.Pets[5];

            // act
            var result = volunteer.MovePet(fifthPet, positionToMove);

            //  assert
            result.IsSuccess.Should().BeTrue();
            firstPet.Position.Value.Should().Be(1);
            secondPet.Position.Value.Should().Be(3);
            thirdPet.Position.Value.Should().Be(4);
            fourthPet.Position.Value.Should().Be(5);
            fifthPet.Position.Value.Should().Be(2);
            sixthPet.Position.Value.Should().Be(6);
        }

        [Fact]
        public void Move_Pet_Should_Move_Other_Pets_Back_When_New_Position_Higher_Than_Current()
        {
            // arrange
            const int petsCount = 6;
            var volunteer = CreateVolunteerWithPets(petsCount);

            var positionToMove = Position.Create(5).Value;

            var firstPet = volunteer.Pets[0];
            var secondPet = volunteer.Pets[1];
            var thirdPet = volunteer.Pets[2];
            var fourthPet = volunteer.Pets[3];
            var fifthPet = volunteer.Pets[4];
            var sixthPet = volunteer.Pets[5];

            // act
            var result = volunteer.MovePet(secondPet, positionToMove);

            //  assert
            result.IsSuccess.Should().BeTrue();
            firstPet.Position.Value.Should().Be(1);
            thirdPet.Position.Value.Should().Be(2);
            fourthPet.Position.Value.Should().Be(3);
            fifthPet.Position.Value.Should().Be(4);
            secondPet.Position.Value.Should().Be(5);
            sixthPet.Position.Value.Should().Be(6);
        }

        [Fact]
        public void Move_Pet_Should_Move_Other_Pets_Back_When_New_Position_Is_First()
        {
            // arrange
            const int petsCount = 6;
            var volunteer = CreateVolunteerWithPets(petsCount);

            var positionToMove = Position.Create(1).Value;

            var firstPet = volunteer.Pets[0];
            var secondPet = volunteer.Pets[1];
            var thirdPet = volunteer.Pets[2];
            var fourthPet = volunteer.Pets[3];
            var fifthPet = volunteer.Pets[4];
            var sixthPet = volunteer.Pets[5];

            // act
            var result = volunteer.MovePet(fifthPet, positionToMove);

            //  assert
            result.IsSuccess.Should().BeTrue();
            firstPet.Position.Value.Should().Be(2);
            secondPet.Position.Value.Should().Be(3);
            thirdPet.Position.Value.Should().Be(4);
            fourthPet.Position.Value.Should().Be(5);
            fifthPet.Position.Value.Should().Be(1);
            sixthPet.Position.Value.Should().Be(6);
        }

        [Fact]
        public void Move_Pet_Should_Move_Other_Pets_Back_When_New_Position_Is_Last()
        {
            // arrange
            const int petsCount = 6;
            var volunteer = CreateVolunteerWithPets(petsCount);

            var positionToMove = Position.Create(petsCount).Value;

            var firstPet = volunteer.Pets[0];
            var secondPet = volunteer.Pets[1];
            var thirdPet = volunteer.Pets[2];
            var fourthPet = volunteer.Pets[3];
            var fifthPet = volunteer.Pets[4];
            var sixthPet = volunteer.Pets[5];

            // act
            var result = volunteer.MovePet(secondPet, positionToMove);

            //  assert
            result.IsSuccess.Should().BeTrue();
            firstPet.Position.Value.Should().Be(1);
            thirdPet.Position.Value.Should().Be(2);
            fourthPet.Position.Value.Should().Be(3);
            fifthPet.Position.Value.Should().Be(4);
            sixthPet.Position.Value.Should().Be(5);
            secondPet.Position.Value.Should().Be(6);
        }

        [Fact]
        public void Move_Pet_When_New_Position_Out_Of_Range()
        {
            // arrange
            const int petsCount = 6;
            var volunteer = CreateVolunteerWithPets(petsCount);

            var positionToMove = Position.Create(21).Value;

            var firstPet = volunteer.Pets[0];
            var secondPet = volunteer.Pets[1];
            var thirdPet = volunteer.Pets[2];
            var fourthPet = volunteer.Pets[3];
            var fifthPet = volunteer.Pets[4];
            var sixthPet = volunteer.Pets[5];

            // act
            var result = volunteer.MovePet(secondPet, positionToMove);

            //  assert
            result.IsSuccess.Should().BeTrue();
            firstPet.Position.Value.Should().Be(1);
            thirdPet.Position.Value.Should().Be(2);
            fourthPet.Position.Value.Should().Be(3);
            fifthPet.Position.Value.Should().Be(4);
            sixthPet.Position.Value.Should().Be(5);
            secondPet.Position.Value.Should().Be(6);
        }

        private Volunteer CreateVolunteer()
        {
            var fullName = FullName.Create("name", "second name", "family name").Value;
            var email = Email.Create("email@mail.ru").Value;
            var description = Description.Create("description").Value;
            var experience = Experience.Create(1).Value;
            var phoneNumber = PhoneNumber.Create("89003565766").Value;
            var volunteer = new Volunteer(
                Guid.NewGuid(),
                fullName,
                email,
                description,
                experience,
                phoneNumber,
                [], []);

            return volunteer;
        }

        private Pet CreatePet()
        {
            var nickName = NickName.Create("nickname").Value;
            var speciesId = SpeciesId.Create(Guid.NewGuid()).Value;
            var breedId = BreedId.Create(Guid.NewGuid()).Value;
            var color = Color.Create("color").Value;
            var isSterilized = IsSterilized.Create(true).Value;
            var isVaccinated = IsVaccinated.Create(true).Value;
            var healthInformation = HealthInformation.Create("information").Value;
            var address = Address.Create("city", "street", "apartment").Value;
            var weight = Weight.Create(20).Value;
            var height = Height.Create(20).Value;
            var description = Description.Create("description").Value;
            var phoneNumber = PhoneNumber.Create("89003565766").Value;
            var birthday = Birthday.Create(new DateTime(2020, 12, 10)).Value;
            var creationDate = CreationDate.Create(new DateTime(2013, 11, 15)).Value;
            var petStatus = PetStatus.Create(Status.FoundHome).Value;

            var petToAdd = new Pet(
                Guid.NewGuid(),
                nickName,
                description,
                speciesId,
                breedId,
                color,
                isSterilized,
                isVaccinated,
                healthInformation,
                address,
                weight,
                height,
                birthday,
                creationDate,
                phoneNumber,
                petStatus,
                []);

            return petToAdd;
        }

        private Volunteer CreateVolunteerWithPets(int petsCount)
        {
            var volunteer = CreateVolunteer();

            var pets = Enumerable.Range(1, petsCount).Select(p =>
            CreatePet()).ToList();

            foreach (var pet in pets)
                volunteer.AddPet(pet);

            return volunteer;
        }
    }
}