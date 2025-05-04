using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Domain.PetsVO
{
    public record PetFile
    {
        public FilePath PathToStorage { get; }

        public PetFile(FilePath pathToStorage)
        {
            PathToStorage = pathToStorage;
        }
    }
}
