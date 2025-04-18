﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.EntitiesHandling.Specieses;
using PetFamily.Application.EntitiesHandling.Volunteers;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;

namespace PetFamily.Application.EntitiesHandling.Pets.Commands.Create
{
    public class CreatePetHandler : ICommandHandler<Guid, CreatePetCommand>
    {
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IReadDbContext _readDbContext;
        private readonly IValidator<CreatePetCommand> _validator;
        private readonly ILogger<CreatePetHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePetHandler(
            IVolunteerRepository volunteerRepository,
            ISpeciesRepository speciesRepository,
            IValidator<CreatePetCommand> validator,
            ILogger<CreatePetHandler> logger,
            IUnitOfWork unitOfWork,
            IReadDbContext readDbContext)
        {
            _volunteerRepository = volunteerRepository;
            _speciesRepository = speciesRepository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _readDbContext = readDbContext;
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

            var species = await _readDbContext.Species
                .FirstOrDefaultAsync(s => s.Id == command.SpeciesId);
            if (species == null)
                return Errors.General.NotFound(command.SpeciesId).ToErrorsList();

            var breed = await _readDbContext.Breeds
                .FirstOrDefaultAsync(b => 
                    b.Id == command.BreedId && b.SpeciesId == command.SpeciesId);
            if (breed == null)
                return Errors.General.NotFound(command.BreedId).ToErrorsList();

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
            var petStatus = PetStatus.Create(command.PetStatus).Value;

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
