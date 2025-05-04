using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.Delete
{
    public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
    {
        public DeleteVolunteerCommandValidator()
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("VolunteerId"));

            RuleFor(d => d.Options)
                .Must(p => Enum.IsDefined(typeof(DeletionOptions), p))
                .WithError(Errors.General.ValueIsInvalid("Deletion options"));
        }
    }
}
