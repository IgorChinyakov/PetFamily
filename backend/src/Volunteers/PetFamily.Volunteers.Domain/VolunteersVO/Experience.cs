using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Domain.VolunteersVO
{
    public record Experience
    {
        public int Value { get; }

        private Experience(int value)
        {
            Value = value;
        }

        public static Result<Experience, Error> Create(int value)
        {
            if (value <= 0)
                return Errors.General.ValueIsInvalid("Experience");

            return new Experience(value);
        }
    }
}
