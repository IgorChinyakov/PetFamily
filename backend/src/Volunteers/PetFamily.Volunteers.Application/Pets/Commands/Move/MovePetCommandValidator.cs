using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.PetsVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Pets.Commands.Move
{
    public class MovePetCommandValidator : AbstractValidator<MovePetCommand>
    {
        public MovePetCommandValidator()
        {
            RuleFor(c => c.VolunteerId)
               .NotEmpty()
               .WithError(Errors.General.ValueIsInvalid("VolunteerId"));

            RuleFor(c => c.PetId)
               .NotEmpty()
               .WithError(Errors.General.ValueIsInvalid("VolunteerId"));

            RuleFor(c => c.PositionToMove)
                .MustBeValueObject(Position.Create);
        }
    }
}
