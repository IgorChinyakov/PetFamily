using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Domain.PetsVO
{
    public class MainPhoto
    {
        public string Path { get; }

        public MainPhoto(string path)
        {
            Path = path;
        }
    }
}
