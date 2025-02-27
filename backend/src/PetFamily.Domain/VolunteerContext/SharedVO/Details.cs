﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.SharedVO
{
    public class Details
    {
        public string Title { get; }
        public string Description { get; }

        private Details(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public static Result<Details, Error> Create(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title) 
                || title.Length > Constants.MAX_LOW_TITLE_LENGTH)
                return Errors.General.ValueIsInvalid("Title");
            if (string.IsNullOrWhiteSpace(description) 
                || description.Length > Constants.MAX_HIGH_TITLE_LENGTH)
                return Errors.General.ValueIsInvalid("Description");

            return new Details(title, description);
        }
    }
}
