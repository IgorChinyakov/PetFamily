using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Domain.Pets.Value_objects
{
    public record PetStatus
    {
        public Status Value { get; set; }

        private PetStatus(Status value)
        {
            Value = value;
        }

        public static Result<PetStatus> Create(Status value) => new PetStatus(value);
    }

    public enum Status
    {
        NeedHelp,
        LookingForHome,
        FoundHome
    }
}
