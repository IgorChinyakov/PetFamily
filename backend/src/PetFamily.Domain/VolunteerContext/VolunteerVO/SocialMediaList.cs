using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO
{
    public record SocialMediaList
    {
        private readonly List<SocialMedia> _socialMedia;
        public IReadOnlyCollection<SocialMedia> SocialMedia => _socialMedia;

        private SocialMediaList(List<SocialMedia> socialMedia)
        {
            _socialMedia = socialMedia.ToList();
        }

        public static Result<SocialMediaList> Create(List<SocialMedia> socialMedia) => new SocialMediaList(socialMedia);
    }
}
