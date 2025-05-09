﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Contracts;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Domain.PetsVO;
using static PetFamily.Volunteers.Domain.PetsVO.PetStatus;

namespace PetFamily.Volunteers.Application.Pets.Commands.UpdateStatus
{
    public class UpdatePetStatusHandler :
        ICommandHandler<UpdatePetStatusCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<UpdatePetStatusCommand> _validator;
        private readonly ILogger<UpdatePetStatusHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePetStatusHandler(
            IVolunteersRepository volunteerRepository,
            IValidator<UpdatePetStatusCommand> validator,
            ILogger<UpdatePetStatusHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Volunteers)] IUnitOfWork unitOfWork)
        {
            _repository = volunteerRepository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            UpdatePetStatusCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var volunteerResult = await _repository
                .GetById(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorsList();

            var petStatus = PetStatus.Create((Status)command.Status).Value;

            var updateResult = volunteerResult.Value
                .UpdatePetStatus(command.PetId, petStatus);

            if (updateResult.IsFailure)
                return updateResult.Error.ToErrorsList();

            await _unitOfWork.SaveChanges();

            _logger.LogInformation("Pet status has been updated. Pet Id: {petId}", updateResult.Value);

            return Result.Success<ErrorsList>();
        }
    }
}
