using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.PetsVO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetFamily.Application.Volunteers.Extensions;

namespace PetFamily.Application.Pets.Move
{
    public class MovePetHandler
    {
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IValidator<MovePetCommand> _validator;
        private readonly ILogger<MovePetHandler> _logger;

        public MovePetHandler(
            IVolunteerRepository volunteerRepository, 
            IValidator<MovePetCommand> validator, 
            ILogger<MovePetHandler> logger)
        {
            _volunteerRepository = volunteerRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            MovePetCommand command, CancellationToken token = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var volunteerResult = await 
                _volunteerRepository.GetById(command.VolunteerId);
            if(volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorsList();

            var petResult = await _volunteerRepository
                .GetPetById(command.VolunteerId, command.PetId, token);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorsList();

            var position = petResult.Value.Position.Value;

            var positionToMove = Position.Create(command.PositionToMove).Value;

            var movementResult = volunteerResult.Value.MovePet(petResult.Value, positionToMove);
            if(movementResult.IsFailure)
                return movementResult.Error.ToErrorsList();

            await _volunteerRepository.Save(volunteerResult.Value, token);

            _logger.LogInformation("Pet with position {position} has been moved to {positionToMove}", 
                position, positionToMove.Value);

            return Result.Success<ErrorsList>();
        }
    }
}
