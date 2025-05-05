using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Domain.SharedVO;
using PetFamily.Volunteers.Domain.PetsVO;
using PetFamily.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Options;

namespace PetFamily.Volunteers.Application.Pets.Commands.UpdateMainInfo
{
    public class UpdatePetMainInfoHandler :
        ICommandHandler<UpdatePetMainInfoCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly ISpeciesReadDbContext _readDbContext;
        private readonly IValidator<UpdatePetMainInfoCommand> _validator;
        private readonly ILogger<UpdatePetMainInfoHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePetMainInfoHandler(
            IVolunteersRepository volunteerRepository,
            IValidator<UpdatePetMainInfoCommand> validator,
            ILogger<UpdatePetMainInfoHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Volunteers)] IUnitOfWork unitOfWork,
            ISpeciesReadDbContext readDbContext)
        {
            _repository = volunteerRepository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _readDbContext = readDbContext;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            UpdatePetMainInfoCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var species = await _readDbContext.Species
                .FirstOrDefaultAsync(s => s.Id == command.SpeciesId, cancellationToken);
            if (species == null)
                return Errors.General.NotFound(command.SpeciesId).ToErrorsList();

            var breed = await _readDbContext.Breeds
                .FirstOrDefaultAsync(b =>
                    b.Id == command.BreedId
                    && b.SpeciesId == command.SpeciesId, cancellationToken);
            if (breed == null)
                return Errors.General.NotFound(command.BreedId).ToErrorsList();

            var volunteerResult = await _repository
                .GetById(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorsList();

            var nickName = NickName.Create(command.NickName).Value;
            var description = Description.Create(command.Description).Value;
            var speciesId = SpeciesId.Create(command.SpeciesId).Value;
            var breedId = BreedId.Create(command.BreedId).Value;
            var color = Color.Create(command.Color).Value;
            var isSterilized = IsSterilized.Create(command.IsSterilized).Value;
            var isVaccinated = IsVaccinated.Create(command.IsVaccinated).Value;
            var healthInforamtion = HealthInformation
                .Create(command.HealthInformation).Value;
            var address = Address.Create(
                command.Address.City,
                command.Address.Street,
                command.Address.Apartment).Value;
            var weight = Weight.Create(command.Weight).Value;
            var height = Height.Create(command.Height).Value;
            var birthday = Birthday.Create(command.Birthday).Value;
            var creationDate = CreationDate.Create(command.CreationDate).Value;
            var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

            var updateResult = volunteerResult.Value
                .UpdatePetMainInfo(
                command.PetId,
                nickName,
                description,
                speciesId,
                breedId,
                color,
                isSterilized,
                isVaccinated,
                healthInforamtion,
                address,
                weight,
                height,
                birthday,
                creationDate,
                phoneNumber);

            if (updateResult.IsFailure)
                return updateResult.Error.ToErrorsList();

            await _unitOfWork.SaveChanges();

            _logger.LogInformation("Pet's main info has been updated. Pet Id: {petId}", updateResult.Value);

            return Result.Success<ErrorsList>();
        }
    }
}
