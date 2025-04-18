﻿using CSharpFunctionalExtensions;
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
        public PetFile(FilePath pathToStorage)
        {
            PathToStorage = pathToStorage;
        }

        public FilePath PathToStorage { get; }
    }
}
