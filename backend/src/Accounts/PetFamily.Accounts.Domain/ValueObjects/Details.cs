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
    }
}
