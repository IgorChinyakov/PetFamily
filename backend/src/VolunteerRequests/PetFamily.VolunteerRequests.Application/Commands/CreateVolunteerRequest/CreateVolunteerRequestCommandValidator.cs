using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Commands.CreateVolunteerRequest
{
    public class CreateVolunteerRequestCommandValidator 
        : AbstractValidator<CreateVolunteerRequestCommand>
    {
        public CreateVolunteerRequestCommandValidator()
        {
            RuleFor(c => c.VolunteerInformation)
                .MustBeValueObject(VolunteerInformation.Create);
        }
    }
}
