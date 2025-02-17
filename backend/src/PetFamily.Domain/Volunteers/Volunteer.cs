﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets;
using PetFamily.Domain.Pets.Value_objects;
using PetFamily.Domain.Volunteers.Value_objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Volunteers
{
    public class Volunteer : Entity<Guid>
    {
        private readonly List<Pet> _pets = [];
        private readonly List<Pet> _allPets = [];

        public FullName FullName { get; private set; }
        public Email Email { get; private set; }
        public GeneralDescription Description { get; private set; }
        public Experience Experience { get; private set; }
        public HelpDetails HelpDetails { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public SocialMediaList SocialMediaList { get; private set; }

        private Volunteer(Guid id) : base(id) { }

        public Volunteer(FullName fullName, 
            Email email, 
            GeneralDescription description, 
            Experience experience, 
            HelpDetails helpDetails, 
            PhoneNumber phoneNumber, 
            SocialMediaList socialMediaList)
        {
            FullName = fullName;
            Email = email;
            Description = description;
            Experience = experience;
            HelpDetails = helpDetails;
            PhoneNumber = phoneNumber;
            SocialMediaList = socialMediaList;
        }

        public int ShelteredPets() => _allPets.Count(p => p.PetStatus.Value == Status.FoundHome);
        public int SeekingHomePets() => _allPets.Count(p => p.PetStatus.Value == Status.LookingForHome);
        public int PetsUnderTreatment() => _allPets.Count(p => p.PetStatus.Value == Status.NeedHelp);
    }
}
