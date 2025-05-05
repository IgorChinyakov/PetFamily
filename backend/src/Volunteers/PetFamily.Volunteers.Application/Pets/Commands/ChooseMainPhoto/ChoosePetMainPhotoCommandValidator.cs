using FluentValidation;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.PetsVO;
using PetFamily.Core.Extensions;

namespace PetFamily.Volunteers.Application.Pets.Commands.ChooseMainPhoto
{
    public class ChoosePetMainPhotoCommandValidator : AbstractValidator<ChoosePetMainPhotoCommand>
    {
        public ChoosePetMainPhotoCommandValidator()
        {
            RuleFor(c => c.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("VolunteerId"));

            RuleFor(c => c.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("petId"));

            RuleFor(c => c.Path)
                .MustBeValueObject(FilePath.Create);
        }
    }
}
