using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Contracts.Requests.Species;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.PetsVO;
using PetFamily.Volunteers.Domain.SharedVO;
using static PetFamily.Volunteers.Domain.PetsVO.PetStatus;

namespace PetFamily.Volunteers.Application.Pets.Commands.Create
{
    public class CreatePetHandler : ICommandHandler<Guid, CreatePetCommand>
    {
        private readonly IVolunteersRepository _volunteerRepository;
        private readonly ISpeciesContract _speciesContract;
        private readonly IValidator<CreatePetCommand> _validator;
        private readonly ILogger<CreatePetHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePetHandler(
            IVolunteersRepository volunteerRepository,
            IValidator<CreatePetCommand> validator,
            ILogger<CreatePetHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Volunteers)] IUnitOfWork unitOfWork,
            ISpeciesContract speciesContract)
        {
            _volunteerRepository = volunteerRepository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _speciesContract = speciesContract;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            CreatePetCommand command, CancellationToken token = default)
        {
            var result = await _validator.ValidateAsync(command, token);
            if (!result.IsValid)
                return result.ToErrorsList();

            var volunteerResult = await
                _volunteerRepository.GetById(command.VolunteerId, token);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorsList();

            var speciesResult = await _speciesContract
                .GetSpeciesById(command.SpeciesId, token);
            if (speciesResult.IsFailure)
                return speciesResult.Error;

            var breedResult = await _speciesContract
                .GetBreedById(command.SpeciesId, command.BreedId);
            if (breedResult.IsFailure)
                return breedResult.Error;

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
            var petStatus = PetStatus.Create((Status)command.PetStatus).Value;

            var creationDate = CreationDate.Create(DateTime.UtcNow).Value;
            var phoneNumber = volunteerResult.Value.PhoneNumber;
            var detailsList = volunteerResult.Value.DetailsList
                .Select(dl => Details.Create(dl.Title, dl.Description).Value);

            var pet = new Pet(
                Guid.Empty,
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
                phoneNumber,
                petStatus,
                detailsList);

            volunteerResult.Value.AddPet(pet);
            await _unitOfWork.SaveChanges(token);

            var petId = pet.Id;

            _logger.LogInformation("Pet {fullNmaeResult} with id {petId} has been created", nickName.Value, petId);

            return petId;
        }
    }
}
