using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.PetsVO
{
    public record File
    {
        public string PathToStorage { get; }

        private File(string pathToStorage)
        {
            PathToStorage = pathToStorage;
        }

        public static Result<File, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid("PathToStorage");

            return new File(value);
        }
    }
}
