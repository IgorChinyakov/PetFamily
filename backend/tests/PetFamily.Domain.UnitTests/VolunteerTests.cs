using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Domain.UnitTests
{
    public class VolunteerTests
    {
        [Fact]
        public async Task Add_pet_should_be_success()
        {
            // arrange
            var fullName = FullName.Create("name", "second name", "family name").Value;
            var email = Email.Create("email@mail.ru").Value;
            var description = Description.Create("description").Value;
            var experience = Experience.Create(1).Value;
            var phoneNumber = PhoneNumber.Create("89003565766").Value;
            var socialMediaList = new List<SocialMedia>();
            var detailsList = new List<Details>();

            var volunteer = new Volunteer(fullName, email, description, experience, phoneNumber, detailsList, socialMediaList);

            var pet = new Pet();

            // act

            // assert
        }
    }
}