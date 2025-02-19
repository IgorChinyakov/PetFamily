using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO
{
    public record HelpDetails
    {
        public string Title { get; }
        public string Description { get; }

        private HelpDetails(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public static Result<HelpDetails> Create(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<HelpDetails>("Title is not supposed to be empty");
            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<HelpDetails>("Description is not supposed to be empty");

            return new HelpDetails(title, description);
        }
    }
}
