﻿using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Domain.PetsVO
{
    public record SpeciesId
    {
        private SpeciesId(Guid id)
        {
            Value = id;
        }

        public Guid Value { get; }

        public static Result<SpeciesId, Error> Create(Guid id)
        {
            if (id == Guid.Empty)
                return Errors.General.ValueIsInvalid("Guid");

            return new SpeciesId(id);
        }
    }
}
