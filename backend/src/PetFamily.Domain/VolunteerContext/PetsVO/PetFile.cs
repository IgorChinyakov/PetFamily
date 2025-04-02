using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.PetsVO
{
    public record PetFile
    {
        public string PathToStorage { get; }

        public PetFile()
        {
        }

        [JsonConstructor]
        private PetFile(string pathToStorage)
        {
            PathToStorage = pathToStorage;
        }

        public static Result<PetFile, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid("PathToStorage");

            return new PetFile(value);
        }
    }
}
