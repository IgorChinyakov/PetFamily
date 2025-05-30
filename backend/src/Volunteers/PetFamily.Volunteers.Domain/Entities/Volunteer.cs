using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.PetsVO;
using PetFamily.Volunteers.Domain.SharedVO;
using PetFamily.Volunteers.Domain.VolunteersVO;
using static PetFamily.Volunteers.Domain.PetsVO.PetStatus;


namespace PetFamily.Volunteers.Domain.Entities
{
    public class Volunteer : SoftDeletableEntity
    {
        private readonly List<Pet> _pets = [];

        public IReadOnlyList<Pet> Pets => _pets;

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
            PhoneNumber phoneNumber) : base(id)
        {
            FullName = fullName;
            Email = email;
            Description = description;
            Experience = experience;
            PhoneNumber = phoneNumber;
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

        //public void UpdateSocialMediaList(IEnumerable<SocialMedia> socialMedia)
        //{
        //    _socialMediaList = socialMedia.ToList();
        //}

        //public void UpdateDetailsList(IEnumerable<Details> details)
        //{
        //    _detailsList = details.ToList();
        //}

        public override void Delete()
        {
            base.Delete();

            foreach (var pet in _pets)
            {
                pet.Delete();
            }
        }

        public void SoftDeletePet(Guid id)
        {
            var petResult = GetPetById(id);
            if (petResult.IsFailure)
                return;

            petResult.Value.Delete();
        }

        public void HardDeletePet(Guid id)
        {
            var petResult = GetPetById(id);
            if (petResult.IsFailure)
                return;

            _pets.Remove(petResult.Value);
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

        public Result<Pet, Error> GetPetById(Guid guid)
        {
            var petResult = _pets.Where(p => p.Id == guid).FirstOrDefault();
            if (petResult == null)
                return Errors.General.NotFound(guid);

            return petResult;
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

        public Result<Guid, Error> UpdatePetMainInfo(
            Guid petId,
            NickName nickName,
            Description description,
            SpeciesId speciesId,
            BreedId breedId,
            Color color,
            IsSterilized isSterilized,
            IsVaccinated isVaccinated,
            HealthInformation healthInformation,
            Address address,
            Weight wieght,
            Height height,
            Birthday birthday,
            CreationDate creationDate,
            PhoneNumber phoneNumber)
        {
            var petResult = GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error;

            petResult.Value.UpdateMainInfo(
                nickName,
                description,
                speciesId,
                breedId,
                color,
                isSterilized,
                isVaccinated,
                healthInformation,
                address,
                wieght,
                height,
                birthday,
                creationDate,
                phoneNumber);

            return petResult.Value.Id;
        }

        public Result<Guid, Error> UpdatePetStatus(
            Guid petId,
            PetStatus petStatus)
        {
            var petResult = GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error;

            if (petResult.Value.PetStatus == petStatus)
                return petResult.Value.Id;

            petResult.Value.UpdatePetStatus(petStatus);

            return petResult.Value.Id;
        }

        public Result<string, Error> ChoosePetMainPhoto(
            Guid petId,
            string path)
        {
            var petResult = GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error;

            var files = petResult.Value.Files.Select(f => f.PathToStorage.Path);

            foreach (var file in files)
            {
                if (path == file)
                {
                    var mainPhoto = new MainPhoto(file);
                    petResult.Value.UpdateMainPhoto(mainPhoto);
                    return path;
                }
            }

            return Errors.General.NotFound();
        }
    }
}
