using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Domain.ValueObjects
{
    public record FullName
    {
        public string Name { get; set; } = string.Empty;

        public string? SecondName { get; set; } = string.Empty;

        public string FamilyName { get; set; } = string.Empty;
    }
}
