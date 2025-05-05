using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;

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
