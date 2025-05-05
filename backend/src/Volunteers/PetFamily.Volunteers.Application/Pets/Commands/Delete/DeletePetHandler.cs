using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.Core.Options;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.FileDtos;
using PetFamily.Files.Application;

namespace PetFamily.Volunteers.Application.Pets.Commands.Delete
{
    public class DeletePetHandler :
        ICommandHandler<Guid, DeletePetCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<DeletePetCommand> _validator;
        private readonly ILogger<DeletePetHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFilesProvider _filesProvider;

        public DeletePetHandler(
            IVolunteersRepository repository,
            IValidator<DeletePetCommand> validator,
            ILogger<DeletePetHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Volunteers)] IUnitOfWork unitOfWork,
            IFilesProvider fileProvider)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _filesProvider = fileProvider;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(DeletePetCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _validator.ValidateAsync(command, cancellationToken);
            if (!result.IsValid)
                return result.ToErrorsList();

            var volunteerResult = await _repository
                .GetById(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorsList();

            var petResult = volunteerResult.Value
                .GetPetById(command.PetId);
            if (petResult.IsFailure)
                return command.PetId;

            var filPaths = petResult.Value.Files.Select(f => f.PathToStorage.Path);

            switch (command.Options)
            {
                case DeletionOptions.Soft:
                    volunteerResult.Value.SoftDeletePet(command.PetId);
                    break;
                case DeletionOptions.Hard:
                    volunteerResult.Value.HardDeletePet(command.PetId);
                    foreach (var file in filPaths)
                        await _filesProvider.RemoveFile(
                            new FileMeta(command.BucketName, file), cancellationToken);
                    break;
                default: throw new NotImplementedException("Invalid deletion options");
            };

            await _unitOfWork.SaveChanges();

            _logger.LogInformation("Pet has been removed. Pet Id: {petId}", command.PetId);

            return command.PetId;
        }
    }
}
