using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.PetsVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Pets.UseCases.Move
{
    public class MovePetCommandValidator : AbstractValidator<MovePetCommand>
    {
        public MovePetCommandValidator() 
        {
            RuleFor(c => c.VolunteerId)
               .NotEmpty()
               .WithError(Errors.General.ValueIsRequired());

            RuleFor(c => c.PetId)
               .NotEmpty()
               .WithError(Errors.General.ValueIsRequired());

            RuleFor(c => c.PositionToMove)
                .MustBeValueObject(p => Position.Create(p));
        }
    }
}
