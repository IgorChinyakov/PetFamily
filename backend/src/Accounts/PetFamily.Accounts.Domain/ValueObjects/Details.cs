using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.ValueObjects
{
    public class Details
    {
        public string Title { get; set; }

        public string Description { get; set; } 

        [JsonConstructor]
        public Details(string title, string description)
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
