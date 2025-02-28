﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets.Value_objects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.Entities
{
    public class Volunteer : Entity<Guid>
    {
        private readonly IReadOnlyList<Pet> _pets = [];
        private readonly IReadOnlyList<SocialMedia> _socialMediaList = [];

        public IReadOnlyList<Pet> Pets => _pets;   
        public IReadOnlyList<SocialMedia> SocialMediaList => _socialMediaList;   
        public FullName FullName { get; private set; }
        public Email Email { get; private set; }
        public Description Description { get; private set; }
        public Experience Experience { get; private set; }
        public Details Details { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }

        private Volunteer(Guid id) : base(id) { }

        public Volunteer(FullName fullName,
            Email email,
            Description description,
            Experience experience,
            Details details,
            PhoneNumber phoneNumber,
            List<SocialMedia> socialMedia)
        {
            FullName = fullName;
            Email = email;
            Description = description;
            Experience = experience;
            Details = details;
            PhoneNumber = phoneNumber;
            _socialMediaList = socialMedia;
        }

        public int ShelteredPets() => _pets.Count(p => p.PetStatus.Value == Status.FoundHome);
        public int SeekingHomePets() => _pets.Count(p => p.PetStatus.Value == Status.LookingForHome);
        public int PetsUnderTreatment() => _pets.Count(p => p.PetStatus.Value == Status.NeedHelp);
    }
}
