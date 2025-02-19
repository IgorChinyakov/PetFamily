using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets.Value_objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO
{
    public record SocialMedia
    {
        public string Title { get; }
        public string Link { get; }

        private SocialMedia(string title, string link)
        {
            Title = title;
            Link = link;
        }

        public static Result<SocialMedia> Create(string title, string link)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<SocialMedia>("City is not supposed to be empty");
            if (string.IsNullOrWhiteSpace(link))
                return Result.Failure<SocialMedia>("Street is not supposed to be empty");

            return new SocialMedia(title, link);
        }
    }
}
