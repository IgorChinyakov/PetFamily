using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.Entities
{
    public class Volunteer : SoftDeletableEntity
    {
        private readonly List<Pet> _pets = [];
        private IReadOnlyList<SocialMedia> _socialMediaList = [];
        private IReadOnlyList<Details> _detailsList = [];

        public IReadOnlyList<Pet> Pets => _pets;
        public IReadOnlyList<SocialMedia> SocialMediaList => _socialMediaList;
        public IReadOnlyList<Details> DetailsList => _detailsList;

        public FullName FullName { get; private set; }
        public Email Email { get; private set; }
        public Description Description { get; private set; }
        public Experience Experience { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }

        private Volunteer(Guid id) : base(id) { }

        public Volunteer(
            FullName fullName,
            Email email,
            Description description,
            Experience experience,
            PhoneNumber phoneNumber,
            IEnumerable<Details> detailsList,
            IEnumerable<SocialMedia> socialMediaList)
        {
            FullName = fullName;
            Email = email;
            Description = description;
            Experience = experience;
            PhoneNumber = phoneNumber;
            _socialMediaList = socialMediaList.ToList();
            _detailsList = detailsList.ToList();
        }

        public int ShelteredPets() => _pets.Count(p => p.PetStatus.Value == Status.FoundHome);
        public int SeekingHomePets() => _pets.Count(p => p.PetStatus.Value == Status.LookingForHome);
        public int PetsUnderTreatment() => _pets.Count(p => p.PetStatus.Value == Status.NeedHelp);

        public void UpdateMainInfo(
            FullName fullName,
            Description description,
            PhoneNumber phoneNumber,
            Email email,
            Experience experience)
        {
            FullName = fullName;
            Description = description;
            PhoneNumber = phoneNumber;
            Email = email;
            Experience = experience;
        }

        public void UpdateSocialMediaList(IEnumerable<SocialMedia> socialMedia)
        {
            _socialMediaList = socialMedia.ToList();
        }

        public void UpdateDetailsList(IEnumerable<Details> details)
        {
            _detailsList = details.ToList();
        }

        public override void Delete()
        {
            base.Delete();

            foreach (var pet in _pets)
            {
                pet.Delete();
            }
        }

        public override void Restore()
        {
            base.Restore();

            foreach (var pet in _pets)
            {
                pet.Restore();
            }
        }

        public UnitResult<Error> AddPet(Pet pet)
        {
            var serialNumberResult = SerialNumber.Create(_pets.Count + 1);
            if (serialNumberResult.IsFailure)
                return serialNumberResult.Error;

            pet.SetSerialNumber(serialNumberResult.Value);

            _pets.Add(pet);

            return Result.Success<Error>();
        }

        public UnitResult<Error> MovePet(SerialNumber currentSerialNumber, SerialNumber serialNumberToMove)
        {
            var firstPet = _pets.Where(p => p.SerialNumber == currentSerialNumber).FirstOrDefault();
            var secondPet = _pets.Where(p => p.SerialNumber == serialNumberToMove).FirstOrDefault();

            if (firstPet == null || secondPet == null)
                return Errors.General.NotFound();

            if(currentSerialNumber == serialNumberToMove)
                return Result.Success<Error>();

            _pets.Remove(firstPet);

            if (currentSerialNumber < serialNumberToMove)
                foreach (var pet in _pets)
                    if (pet.SerialNumber > currentSerialNumber && pet.SerialNumber <= serialNumberToMove)
                        pet.SetSerialNumber(SerialNumber.Create(pet.SerialNumber - 1).Value);

            if (currentSerialNumber > serialNumberToMove)
                foreach (var pet in _pets)
                    if (pet.SerialNumber >= serialNumberToMove && pet.SerialNumber < currentSerialNumber)
                        pet.SetSerialNumber(SerialNumber.Create(pet.SerialNumber + 1).Value);

            firstPet.SetSerialNumber(serialNumberToMove);
            _pets.Add(firstPet);

            _pets.Sort((firstPet, secondPet) => firstPet.SerialNumber.Value.CompareTo(secondPet.SerialNumber.Value));

            return Result.Success<Error>();
        }

        public UnitResult<Error> MovePetToBeginning(SerialNumber serialNumber)
            => MovePet(serialNumber, SerialNumber.Create(1).Value);

        public UnitResult<Error> MovePetToEnd(SerialNumber serialNumber)
            => MovePet(serialNumber, SerialNumber.Create(_pets.Count + 1).Value);
    }
}
