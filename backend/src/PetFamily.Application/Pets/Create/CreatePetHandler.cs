using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Specieses;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Application.Volunteers.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Pets.Create
{
    public class CreatePetHandler
    {
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IValidator<CreatePetCommand> _validator;
        private readonly ILogger<CreatePetHandler> _logger;

        public CreatePetHandler(
            IVolunteerRepository volunteerRepository,
            ISpeciesRepository speciesRepository,
            IValidator<CreatePetCommand> validator,
            ILogger<CreatePetHandler> logger)
        {
            _volunteerRepository = volunteerRepository;
            _speciesRepository = speciesRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            CreatePetCommand command, CancellationToken token = default)
        {
            var result = await _validator.ValidateAsync(command, token);
            if (!result.IsValid)
                return result.ToErrorsList();

            var getVolunteerResult = await 
                _volunteerRepository.GetById(command.VolunteerId, token);
            if (getVolunteerResult.IsFailure)
                return getVolunteerResult.Error.ToErrorsList();

            var getSpeciesResult = await 
                _speciesRepository.GetById(command.SpeciesId, command.BreedId, token);
            if (getSpeciesResult.IsFailure)
                return getSpeciesResult.Error.ToErrorsList();

            var nickName = NickName.Create(command.NickName).Value;
            var description = Description.Create(command.Description).Value;
            var speciesId = SpeciesId.Create(command.SpeciesId).Value;
            var breedId = BreedId.Create(command.BreedId).Value;
            var color = Color.Create(command.Color).Value;
            var isSterilized = IsSterilized.Create(command.IsSterilized).Value;
            var isVaccinated = IsVaccinated.Create(command.IsVaccinated).Value;
            var healthInforamtion = HealthInformation.Create(command.HealthInformation).Value;
            var address = Address.Create(command.Address.City, command.Address.Street, command.Address.Apartment).Value;
            var weight = Weight.Create(command.Weight).Value;
            var height = Height.Create(command.Height).Value;
            var birthday = Birthday.Create(command.Birthday).Value;
            var petStatus = PetStatus.Create(command.PetStatus).Value;

            var creationDate = CreationDate.Create(DateTime.UtcNow).Value;
            var phoneNumber = getVolunteerResult.Value.PhoneNumber;
            var detailsList = getVolunteerResult.Value.DetailsList;

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

            getVolunteerResult.Value.AddPet(pet);
            await _volunteerRepository.Save(getVolunteerResult.Value, token);

            var petId = pet.Id;

            _logger.LogInformation("Pet {fullNmaeResult} with id {petId} has been created", nickName.Value, petId);

            return petId;
        }
    }
}
