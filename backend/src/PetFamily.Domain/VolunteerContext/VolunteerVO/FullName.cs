using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets.Value_objects;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO
{
    public record FullName
    {
        public const int MAX_LENGTH = 50;

        public string Name { get; }
        public string? SecondName { get; }
        public string FamilyName { get; }

        private FullName(string name, string? secondName, string familyName)
        {
            Name = name;
            SecondName = secondName;
            FamilyName = familyName;
        }

        public static Result<FullName, Error> Create(string name, string? secondName, string familyName)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_LENGTH)
                return Errors.General.ValueIsInvalid("Name");
            if (string.IsNullOrWhiteSpace(secondName) || secondName.Length > MAX_LENGTH)
                return Errors.General.ValueIsInvalid("Second name");
            if (string.IsNullOrWhiteSpace(familyName) || familyName.Length > MAX_LENGTH)
                return Errors.General.ValueIsInvalid("Family name");

            return new FullName(name, secondName, familyName);
        }
    }
}
