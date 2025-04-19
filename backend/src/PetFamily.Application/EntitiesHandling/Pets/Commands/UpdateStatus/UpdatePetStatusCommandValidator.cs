using FluentValidation;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateStatus
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
