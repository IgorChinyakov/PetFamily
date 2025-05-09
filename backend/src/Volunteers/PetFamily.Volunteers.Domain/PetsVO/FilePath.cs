using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Domain.PetsVO
{
    public class FilePath
    {
        public string Path { get; }

        private FilePath(string path)
        {
            Path = path;
        }

        public static Result<FilePath, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsInvalid("PathToStorage");

            return new FilePath(value);
        }
    }
}
