using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.Delete
{
    public class DeleteVolunteerHandler
    {
        private readonly IVolunteerRepository _repository;
        private readonly IValidator<DeleteVolunteerCommand> _validator;
        private readonly ILogger<DeleteVolunteerHandler> _logger;

        public DeleteVolunteerHandler(
            IVolunteerRepository repository,
            IValidator<DeleteVolunteerCommand> validator,
            ILogger<DeleteVolunteerHandler> logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            DeleteVolunteerCommand command,
            CancellationToken token = default)
        {
            var result = await _validator.ValidateAsync(command, token);

            if (!result.IsValid)
                return result.ToErrorsList();

            var volunteerResult = await _repository.GetById(command.Id, token);

            if (volunteerResult.IsFailure)
                return Errors.General.ValueIsInvalid("VolunteerId").ToErrorsList();

            var volunteerId = command.Options switch
            {
                DeletionOptions.Soft => await _repository.SoftDelete(volunteerResult.Value, token),
                DeletionOptions.Hard => await _repository.HardDelete(volunteerResult.Value, token),
                _ => throw new NotImplementedException("Invalid deletion options")
            };
            
            _logger.LogInformation("Volunteer has been removed. Volunteer Id: {volunteerId}", volunteerId);

            return volunteerId;
        }
    }

    public enum DeletionOptions
    {
        Soft,
        Hard
    }
}
