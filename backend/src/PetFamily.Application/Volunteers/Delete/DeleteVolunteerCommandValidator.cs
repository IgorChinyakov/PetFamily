using FluentValidation;
using PetFamily.Application.Volunteers.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete
{
    public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
    {
        public DeleteVolunteerCommandValidator()
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(d => d.Options)
                .IsInEnum()
                .WithError(Errors.General.ValueIsInvalid("Deletion options"));
        }
    }
}
