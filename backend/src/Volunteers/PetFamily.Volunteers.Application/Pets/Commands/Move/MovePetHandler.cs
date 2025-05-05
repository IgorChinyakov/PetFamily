using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Domain.PetsVO;
using PetFamily.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Options;

namespace PetFamily.Volunteers.Application.Pets.Commands.Move
{
    public class MovePetHandler : ICommandHandler<MovePetCommand>
    {
        private readonly IVolunteersRepository _volunteerRepository;
        private readonly IValidator<MovePetCommand> _validator;
        private readonly ILogger<MovePetHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public MovePetHandler(
            IVolunteersRepository volunteerRepository,
            IValidator<MovePetCommand> validator,
            ILogger<MovePetHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Volunteers)] IUnitOfWork unitOfWork)
        {
            _volunteerRepository = volunteerRepository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            MovePetCommand command, CancellationToken token = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var volunteerResult = await
                _volunteerRepository.GetById(command.VolunteerId);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorsList();

            var petResult = await _volunteerRepository
                .GetPetById(command.VolunteerId, command.PetId, token);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorsList();

            var position = petResult.Value.Position.Value;

            var positionToMove = Position.Create(command.PositionToMove).Value;

            var movementResult = volunteerResult.Value.MovePet(petResult.Value, positionToMove);
            if (movementResult.IsFailure)
                return movementResult.Error.ToErrorsList();

            await _unitOfWork.SaveChanges(token);

            _logger.LogInformation("Pet with position {position} has been moved to {positionToMove}",
                position, positionToMove.Value);

            return Result.Success<ErrorsList>();
        }
    }
}
