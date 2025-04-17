using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.EntitiesHandling.Pets.Commands.Create
{
    public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
    {
        public CreatePetCommandValidator()
        {
            RuleFor(c => c.Address)
                .MustBeValueObject(a => Address.Create(a.City, a.Street, a.Apartment));

            RuleFor(c => c.Birthday)
                .MustBeValueObject(a => Birthday.Create(a));

            RuleFor(c => c.BreedId)
                .MustBeValueObject(b => BreedId.Create(b));

            RuleFor(c => c.SpeciesId)
                .MustBeValueObject(s => SpeciesId.Create(s));

            RuleFor(c => c.Color)
                .MustBeValueObject(c => Color.Create(c));

            RuleFor(c => c.Description)
                .MustBeValueObject(d => Description.Create(d));

            RuleFor(c => c.HealthInformation)
                .MustBeValueObject(h => HealthInformation.Create(h));

            RuleFor(c => c.Height)
                .MustBeValueObject(h => Height.Create(h));

            RuleFor(c => c.NickName)
                .MustBeValueObject(n => NickName.Create(n));

            RuleFor(c => c.Weight)
                .MustBeValueObject(w => Weight.Create(w));

            RuleFor(c => c.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(c => c.PetStatus).Must(p => Enum.IsDefined(typeof(Status), p));
        }
    }
}
