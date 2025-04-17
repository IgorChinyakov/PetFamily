using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.EntitiesHandling.Specieses.Commands.Create;
using PetFamily.Application.EntitiesHandling.Specieses;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetFamily.Application.Extensions;

namespace PetFamily.Application.EntitiesHandling.Breeds.Commands.Create
{
    public class CreateBreedHandler : ICommandHandler<Guid, CreateBreedCommand>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IValidator<CreateBreedCommand> _validator;
        private readonly ILogger<CreateBreedHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBreedHandler(
            ISpeciesRepository repository,
            IValidator<CreateBreedCommand> validator,
            ILogger<CreateBreedHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            CreateBreedCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _validator.ValidateAsync(command, cancellationToken);
            if (!result.IsValid)
                return result.ToErrorsList();

            var species = await _repository.GetById(command.SpeciesId); 
            if (species.IsFailure)
                return result.ToErrorsList();

            var name = Name.Create(command.Name).Value;

            var breed = new Breed(Guid.Empty, name);

            species.Value.AddBreed(breed);
            await _unitOfWork.SaveChanges();

            _logger.LogInformation("Breed {name} with id {id} has been created", name, breed.Id);

            return breed.Id;
        }
    }
}
