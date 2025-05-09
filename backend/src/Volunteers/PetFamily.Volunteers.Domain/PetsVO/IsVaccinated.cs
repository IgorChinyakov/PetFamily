using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Domain.PetsVO
{
    public record IsVaccinated
    {
        public bool Value { get; }

        private IsVaccinated(bool value)
        {
            Value = value;
        }

        public static Result<IsVaccinated> Create(bool value) => new IsVaccinated(value);
    }
}
