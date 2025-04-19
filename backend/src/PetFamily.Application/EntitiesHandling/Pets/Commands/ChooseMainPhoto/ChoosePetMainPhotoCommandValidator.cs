using FluentValidation;
using PetFamily.Api.Requests.Pets;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.PetsVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.EntitiesHandling.Pets.Commands.ChooseMainPhoto
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
