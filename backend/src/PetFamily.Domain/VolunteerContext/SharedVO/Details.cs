using CSharpFunctionalExtensions;
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

        public static Result<Details> Create(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Details>("Title is not supposed to be empty");
            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<Details>("Description is not supposed to be empty");

            return new Details(title, description);
        }
    }
}
