﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record NickName
    {
        public const int MAX_LENGTH = 40;
        public string Value { get; }

        private NickName(string value)
        {
            Value = value;
        }

        public static Result<NickName, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_LENGTH)
                return Errors.General.ValueIsInvalid(value);

            return new NickName(value);
        }
    }
}
