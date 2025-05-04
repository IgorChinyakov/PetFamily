using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Pets.Commands.Delete
{
    public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
    {
        public DeletePetCommandValidator()
        {
            RuleFor(d => d.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("PetId"));

            RuleFor(d => d.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("VolunteerId"));

            RuleFor(d => d.Options)
                .Must(p => Enum.IsDefined(typeof(DeletionOptions), p))
                .WithError(Errors.General.ValueIsInvalid("Deletion options"));
        }
    }
}
