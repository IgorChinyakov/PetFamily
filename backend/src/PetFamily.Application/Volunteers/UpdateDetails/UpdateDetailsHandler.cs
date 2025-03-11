﻿using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Volunteers.Extensions;
using PetFamily.Application.Volunteers.UpdateSocialMedia;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.UpdateDetails
{
    public class UpdateDetailsHandler
    {
        private readonly IVolunteerRepository _repository;
        private readonly IValidator<UpdateDetailsCommand> _validator;

        public UpdateDetailsHandler(
            IVolunteerRepository repository,
            IValidator<UpdateDetailsCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            UpdateDetailsCommand command,
            CancellationToken token = default)
        {
            var validationResult = await _validator.ValidateAsync(command, token);

            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var volunteerResult = await _repository.GetById(command.Id);
            if (volunteerResult.IsFailure)
                return Errors.General.ValueIsInvalid("VolunteerId").ToErrorsList();

            var details = command.Details.Select(sm => Details.Create(sm.Title, sm.Description).Value);
            volunteerResult.Value.UpdateDetailsList(details);
            await _repository.Save(volunteerResult.Value, token);

            return volunteerResult.Value.Id;
        }
    }
}
