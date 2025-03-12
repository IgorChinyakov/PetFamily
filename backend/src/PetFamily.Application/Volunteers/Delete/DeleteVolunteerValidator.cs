using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.Validation;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.Delete
{
    public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
    {
        public DeleteVolunteerValidator()
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
        }
    }
}
