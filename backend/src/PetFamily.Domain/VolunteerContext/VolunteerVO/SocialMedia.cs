﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Pets.Value_objects;
using PetFamily.Domain.Shared;
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

        public SocialMedia()
        {
        }

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
