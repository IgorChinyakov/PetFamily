using FluentValidation;
using PetFamily.Core.DTOs;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.PetsVO;
using static PetFamily.Volunteers.Domain.PetsVO.PetStatus;

namespace PetFamily.Volunteers.Application.Pets.Commands.UpdateStatus
{
    public class UpdatePetStatusCommandValidator :
        AbstractValidator<UpdatePetStatusCommand>
    {
        public UpdatePetStatusCommandValidator()
        {
            RuleFor(c => c.Status)
               .Must(p => Enum.IsDefined(typeof(PetStatusDto), p))
               .WithError(Errors.General.ValueIsInvalid("PetStatus"));
        }
    }
}
