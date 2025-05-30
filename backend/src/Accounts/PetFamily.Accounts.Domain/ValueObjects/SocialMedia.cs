using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.ValueObjects
{
    public record SocialMedia
    {
        public string Title { get; set; }

        public string Link { get; set; }

        [JsonConstructor]
        public SocialMedia(string title, string link)
        {
            Title = title;
            Link = link;
        }
    }
}
