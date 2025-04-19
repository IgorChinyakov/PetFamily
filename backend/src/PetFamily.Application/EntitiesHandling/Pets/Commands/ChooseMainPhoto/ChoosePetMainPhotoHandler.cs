using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Api.Requests.Pets;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.EntitiesHandling.Pets.Commands.UpdateMainInfo;
using PetFamily.Application.EntitiesHandling.Volunteers;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.EntitiesHandling.Pets.Commands.ChooseMainPhoto
{
    public class ChoosePetMainPhotoHandler : 
        ICommandHandler<string, ChoosePetMainPhotoCommand>
    {
        private readonly IVolunteerRepository _repository;
        private readonly IValidator<ChoosePetMainPhotoCommand> _validator;
        private readonly ILogger<ChoosePetMainPhotoHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ChoosePetMainPhotoHandler(
            IVolunteerRepository volunteerRepository,
            IValidator<ChoosePetMainPhotoCommand> validator,
            ILogger<ChoosePetMainPhotoHandler> logger,
            IUnitOfWork unitOfWork)
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

            if(chooseMainPhotoResult.IsFailure)
                return chooseMainPhotoResult.Error.ToErrorsList();

            await _unitOfWork.SaveChanges();

            return chooseMainPhotoResult.Value;
        }
    }
}
