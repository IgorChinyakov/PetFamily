using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Specieses.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Specieses.Application.Breeds.Commands.Create
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
