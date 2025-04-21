using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateMainInfo
{
    public class UpdatePetMainInfoCommandValidator : AbstractValidator<UpdatePetMainInfoCommand>
    {
        public UpdatePetMainInfoCommandValidator()
        {
            RuleFor(c => c.Address)
    .MustBeValueObject(a => Address.Create(a.City, a.Street, a.Apartment));

            RuleFor(c => c.Birthday)
                .MustBeValueObject(Birthday.Create);

            RuleFor(c => c.BreedId)
                .MustBeValueObject(BreedId.Create);

            RuleFor(c => c.SpeciesId)
                .MustBeValueObject(SpeciesId.Create);

            RuleFor(c => c.Color)
                .MustBeValueObject(Color.Create);

            RuleFor(c => c.Description)
                .MustBeValueObject(Description.Create);

            RuleFor(c => c.HealthInformation)
                .MustBeValueObject(HealthInformation.Create);

            RuleFor(c => c.Height)
                .MustBeValueObject(Height.Create);

            RuleFor(c => c.NickName)
                .MustBeValueObject(NickName.Create);

            RuleFor(c => c.Weight)
                .MustBeValueObject(Weight.Create);

            RuleFor(c => c.PhoneNumber)
                .MustBeValueObject(PhoneNumber.Create);

            RuleFor(c => c.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsInvalid("VolunteerId"));
        }
    }
}
