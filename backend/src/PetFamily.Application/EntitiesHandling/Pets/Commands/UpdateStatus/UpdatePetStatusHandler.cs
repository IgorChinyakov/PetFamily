using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.EntitiesHandling.Volunteers;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateStatus
{
    public class UpdatePetStatusHandler : 
        ICommandHandler<UpdatePetStatusCommand>
    {
        private readonly IVolunteerRepository _repository;
        private readonly IReadDbContext _readDbContext;
        private readonly IValidator<UpdatePetStatusCommand> _validator;
        private readonly ILogger<UpdatePetStatusHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePetStatusHandler(
            IVolunteerRepository volunteerRepository,
            IValidator<UpdatePetStatusCommand> validator,
            ILogger<UpdatePetStatusHandler> logger,
            IUnitOfWork unitOfWork,
            IReadDbContext readDbContext)
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
