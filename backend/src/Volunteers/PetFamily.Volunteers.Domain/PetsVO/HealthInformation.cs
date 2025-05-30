﻿using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Domain.PetsVO
{
    public record HealthInformation
    {
        public const int MAX_LENGTH = 1500;

        public string Value { get; }

        private HealthInformation(string value)
        {
            Value = value;
        }

        public static Result<HealthInformation, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
                return Errors.General.ValueIsInvalid("Health information");

            return new HealthInformation(value);
        }
    }
}
