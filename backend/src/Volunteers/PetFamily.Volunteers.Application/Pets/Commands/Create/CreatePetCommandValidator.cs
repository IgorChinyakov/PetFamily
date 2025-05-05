using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.PetsVO;
using PetFamily.Volunteers.Domain.SharedVO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetFamily.Volunteers.Domain.PetsVO.PetStatus;

namespace PetFamily.Volunteers.Application.Pets.Commands.Create
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
                .WithError(Errors.General.ValueIsInvalid("VolunteerId"));

            RuleFor(c => c.PetStatus)
                .Must(p => Enum.IsDefined(typeof(Status), p))
                .WithError(Errors.General.ValueIsInvalid("PetStatus"));
        }
    }
}
