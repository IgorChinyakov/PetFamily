using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application.Database;
using PetFamily.Specieses.Domain.Entities;
using PetFamily.Specieses.Domain.ValueObjects;

namespace PetFamily.Specieses.Application.Breeds.Commands.Create
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
            [FromKeyedServices(UnitOfWorkKeys.Species)] IUnitOfWork unitOfWork)
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
