using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            Guid id,
            FullName fullName,
            Email email,
            Description description,
            Experience experience,
            PhoneNumber phoneNumber,
            IEnumerable<Details> detailsList,
            IEnumerable<SocialMedia> socialMediaList) : base(id)
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
            var positionResult = Position.Create(_pets.Count + 1);
            if (positionResult.IsFailure)
                return positionResult.Error;

            pet.SetPosition(positionResult.Value);

            _pets.Add(pet);

            return Result.Success<Error>();
        }

        public UnitResult<Error> MovePet(Pet petToMove, Position positionToMove)
        {
            var currentPosition = petToMove.Position;
            if (currentPosition == positionToMove || _pets.Count == 1)
                return Result.Success<Error>();

            var adjustedPosition = AdjustPostionIfOutOfRange(positionToMove);
            if (adjustedPosition.IsFailure)
                return adjustedPosition.Error;

            var moveResult = MovePetsBetweenPositions(adjustedPosition.Value, currentPosition);
            if (moveResult.IsFailure)
                return moveResult.Error;

            petToMove.SetPosition(adjustedPosition.Value);

            return Result.Success<Error>();
        }

        private UnitResult<Error> MovePetsBetweenPositions(Position positionToMove, Position currentPosition)
        {
            if (currentPosition < positionToMove)
            {
                var petsToMove = _pets.Where(
                    p => p.Position <= positionToMove && p.Position > currentPosition);

                foreach (var pet in petsToMove)
                {
                    var result = pet.MoveBack();
                    if (result.IsFailure)
                        return result.Error;
                }
            }
            else if (currentPosition > positionToMove)
            {
                var petsToMove = _pets.Where(
                    p => p.Position < currentPosition && p.Position >= positionToMove);

                foreach (var pet in petsToMove)
                {
                    var result = pet.MoveForward();
                    if (result.IsFailure)
                        return result.Error;
                }
            }

            return Result.Success<Error>();
        }

        private Result<Position, Error> AdjustPostionIfOutOfRange(Position positionToMove)
        {
            if (positionToMove.Value <= _pets.Count)
                return positionToMove;

            var lastPosition = Position.Create(_pets.Count);
            if (lastPosition.IsFailure)
                return lastPosition.Error;

            return lastPosition.Value;
        }

        public Result<Pet, Error> GetPetById(Guid guid)
        {
            var petResult = _pets.Where(p => p.Id == guid).FirstOrDefault();
            if (petResult == null)
                return Errors.General.NotFound(guid);

            return petResult;
        }
    }
}
