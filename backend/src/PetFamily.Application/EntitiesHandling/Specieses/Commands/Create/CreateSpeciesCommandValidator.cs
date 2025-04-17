using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.SpeciesContext.ValueObjects;

namespace PetFamily.Application.EntitiesHandling.Specieses.Commands.Create
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
