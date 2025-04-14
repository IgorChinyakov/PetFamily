using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.Delete
{
    public class DeleteVolunteerHandler : ICommandHandler<Guid, DeleteVolunteerCommand>
    {
        private readonly IVolunteerRepository _repository;
        private readonly IValidator<DeleteVolunteerCommand> _validator;
        private readonly ILogger<DeleteVolunteerHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteVolunteerHandler(
            IVolunteerRepository repository,
            IValidator<DeleteVolunteerCommand> validator,
            ILogger<DeleteVolunteerHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
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
                DeletionOptions.Soft => _repository.SoftDelete(volunteerResult.Value, token),
                DeletionOptions.Hard => _repository.HardDelete(volunteerResult.Value, token),
                _ => throw new NotImplementedException("Invalid deletion options")
            };

            await _unitOfWork.SaveChanges();
            
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
