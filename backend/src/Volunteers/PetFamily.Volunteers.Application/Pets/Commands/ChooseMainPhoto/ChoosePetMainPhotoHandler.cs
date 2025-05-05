using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application;

namespace PetFamily.Volunteers.Application.Pets.Commands.ChooseMainPhoto
{
    public class ChoosePetMainPhotoHandler :
        ICommandHandler<string, ChoosePetMainPhotoCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<ChoosePetMainPhotoCommand> _validator;
        private readonly ILogger<ChoosePetMainPhotoHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ChoosePetMainPhotoHandler(
            IVolunteersRepository volunteerRepository,
            IValidator<ChoosePetMainPhotoCommand> validator,
            ILogger<ChoosePetMainPhotoHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Volunteers)] IUnitOfWork unitOfWork)
        {
            _repository = volunteerRepository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string, ErrorsList>> Handle(
            ChoosePetMainPhotoCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _validator.ValidateAsync(command, cancellationToken);
            if (!result.IsValid)
                return result.ToErrorsList();

            var volunteerResult = await
                _repository.GetById(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorsList();

            var chooseMainPhotoResult = volunteerResult.Value
                .ChoosePetMainPhoto(command.PetId, command.Path);

            if (chooseMainPhotoResult.IsFailure)
                return chooseMainPhotoResult.Error.ToErrorsList();

            await _unitOfWork.SaveChanges();

            return chooseMainPhotoResult.Value;
        }
    }
}
