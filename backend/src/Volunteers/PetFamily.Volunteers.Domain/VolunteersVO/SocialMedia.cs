﻿using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using System.Text.Json.Serialization;

namespace PetFamily.Volunteers.Domain.VolunteersVO
{
    public record SocialMedia
    {
        public string Title { get; }
        public string Link { get; }

        public SocialMedia()
        {
        }

        [JsonConstructor]
        private SocialMedia(string title, string link)
        {
            Title = title;
            Link = link;
        }

        public static Result<SocialMedia, Error> Create(string title, string link)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Errors.General.ValueIsInvalid("Title");
            if (string.IsNullOrWhiteSpace(link))
                return Errors.General.ValueIsInvalid("Link");

            return new SocialMedia(title, link);
        }
    }
}
