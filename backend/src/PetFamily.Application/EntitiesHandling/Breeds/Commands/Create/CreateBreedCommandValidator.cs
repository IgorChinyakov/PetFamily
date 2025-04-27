using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.SpeciesContext.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.EntitiesHandling.Breeds.Commands.Create
{
    public class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
    {
        public CreateBreedCommandValidator()
        {
            RuleFor(c => c.Name)
                .MustBeValueObject(Name.Create);
        }
    }
}
