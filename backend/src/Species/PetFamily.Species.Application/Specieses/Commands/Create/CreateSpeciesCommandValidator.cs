using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Specieses.Domain.ValueObjects;

namespace PetFamily.Specieses.Application.Specieses.Commands.Create
{
    public class CreateSpeciesCommandValidator : AbstractValidator<CreateSpeciesCommand>
    {
        public CreateSpeciesCommandValidator()
        {
            RuleFor(c => c.Name)
                .MustBeValueObject(Name.Create);
        }
    }
}
