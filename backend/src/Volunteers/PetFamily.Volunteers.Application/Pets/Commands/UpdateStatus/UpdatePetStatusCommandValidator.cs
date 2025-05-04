using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Volunteers.Domain.PetsVO;

namespace PetFamily.Volunteers.Application.Pets.Commands.UpdateStatus
{
    public class UpdatePetStatusCommandValidator :
        AbstractValidator<UpdatePetStatusCommand>
    {
        public UpdatePetStatusCommandValidator()
        {
            RuleFor(c => c.Status)
               .Must(p => Enum.IsDefined(typeof(Status), p))
               .WithError(Errors.General.ValueIsInvalid("PetStatus"));
        }
    }
}
