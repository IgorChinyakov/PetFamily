using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.EntitiesHandling.Specieses.Commands.Create;
using PetFamily.Application.EntitiesHandling.Volunteers.Commands.Create;
using PetFamily.Application.EntitiesHandling.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Application.EntitiesHandling.Specieses;
using PetFamily.Application.Extensions;
using PetFamily.Domain.SpeciesContext.ValueObjects;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.VolunteerContext.Entities;

namespace PetFamily.Api.Controllers
{
    public class CreateSpeciesHandler : 
        ICommandHandler<Guid, CreateSpeciesCommand>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IValidator<CreateSpeciesCommand> _validator;
        private readonly ILogger<CreateSpeciesHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSpeciesHandler(
            ISpeciesRepository repository,
            IValidator<CreateSpeciesCommand> validator,
            ILogger<CreateSpeciesHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            CreateSpeciesCommand command, 
            CancellationToken cancellationToken = default)
        {
            var result = await _validator.ValidateAsync(command, cancellationToken);
            if (!result.IsValid)
                return result.ToErrorsList();

            var name = Name.Create(command.Name).Value;

            var species = new Species(Guid.NewGuid(), name);

            var addResult = await _repository.Add(species, cancellationToken);
            await _unitOfWork.SaveChanges();

            _logger.LogInformation("Species {name} with id {id} has been created", name, addResult);

            return addResult;
        }
    }
}