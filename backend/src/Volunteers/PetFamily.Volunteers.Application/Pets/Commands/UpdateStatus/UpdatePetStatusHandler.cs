using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Domain.PetsVO;

namespace PetFamily.Volunteers.Application.Pets.Commands.UpdateStatus
{
    public class UpdatePetStatusHandler :
        ICommandHandler<UpdatePetStatusCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly ISpeciesReadDbContext _readDbContext;
        private readonly IValidator<UpdatePetStatusCommand> _validator;
        private readonly ILogger<UpdatePetStatusHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePetStatusHandler(
            IVolunteersRepository volunteerRepository,
            IValidator<UpdatePetStatusCommand> validator,
            ILogger<UpdatePetStatusHandler> logger,
            IUnitOfWork unitOfWork,
            ISpeciesReadDbContext readDbContext)
        {
            _repository = volunteerRepository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _readDbContext = readDbContext;
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

            var petStatus = PetStatus.Create(command.Status).Value;

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
