using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets.Value_objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Volunteers.Value_objects
{
    public record FullName
    {
        public string Name { get;}
        public string SecondName { get;}
        public string FamilyName { get;}

        private FullName(string name, string secondName, string familyName)
        {
            Name = name;
            SecondName = secondName;  
            FamilyName = familyName;
        }

        public static Result<FullName> Create(string name, string secondName, string familyName)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<FullName>("City is not supposed to be empty");
            if (string.IsNullOrWhiteSpace(secondName))
                return Result.Failure<FullName>("Street is not supposed to be empty");
            if (string.IsNullOrWhiteSpace(familyName))
                return Result.Failure<FullName>("House is not supposed to be empty");

            return new FullName(name, secondName, familyName);
        }
    }
}
