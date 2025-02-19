using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets.Value_objects;
using PetFamily.Domain.VolunteerContext.SharedVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.Entities
{
    public class Pet : Entity<Guid>
    {
        public NickName NickName { get; private set; }
        public Description Description { get; private set; }
        public SpeciesId SpeciesId { get; private set; }
        public BreedId BreedId { get; private set; }
        public Color Color { get; private set; }
        public IsSterilized IsSterilized { get; private set; }
        public IsVaccinated IsVaccinated { get; private set; }
        public HealthInformation HealthInformation { get; private set; }
        public Address Address { get; private set; }
        public Weight Weight { get; private set; }
        public Height Height { get; private set; }
        public Birthday Birthday { get; private set; }
        public PhoneNumber OwnerPhoneNumber { get; private set; }
        public PetStatus PetStatus { get; private set; }
        public Details Details { get; private set; }

        private Pet(Guid id) : base(id)
        {
        }

        public Pet(Guid id,
            NickName nickName,
            Description description,
            SpeciesId speciesId,
            BreedId breedId,
            Color color,
            IsSterilized isSterilized,
            IsVaccinated isVaccinated,
            HealthInformation healthInformation,
            Address address,
            Weight weight,
            Height height,
            Birthday birthday,
            PhoneNumber ownerPhoneNumber,
            PetStatus petStatus,
            Details details) : base(id)
        {
            NickName = nickName;
            Description = description;
            SpeciesId = speciesId;
            BreedId = breedId;
            Color = color;
            IsSterilized = isSterilized;
            IsVaccinated = isVaccinated;
            HealthInformation = healthInformation;
            Address = address;
            Weight = weight;
            Height = height;
            Birthday = birthday;
            OwnerPhoneNumber = ownerPhoneNumber;
            PetStatus = petStatus;
            Details = details;
        }
    }
}
