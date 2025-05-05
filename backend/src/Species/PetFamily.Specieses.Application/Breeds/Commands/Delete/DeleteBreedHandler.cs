using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application.Specieses;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests.Pets;

namespace PetFamily.Specieses.Application.Breeds.Commands.Delete
{
    public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpeciesReadDbContext _readDbContext;
        private readonly IVolunteersContract _volunteersContract;

        public DeleteBreedHandler(
            ISpeciesRepository repository,
            [FromKeyedServices(UnitOfWorkKeys.Species)] IUnitOfWork unitOfWork,
            ISpeciesReadDbContext readDbContext,
            IVolunteersContract volunteersContract)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _readDbContext = readDbContext;
            _volunteersContract = volunteersContract;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            DeleteBreedCommand command,
            CancellationToken cancellationToken = default)
        {
            var breedResult = await _repository
                .GetBreedById(command.SpeciesId, command.BreedId, cancellationToken);
            if (breedResult.IsFailure)
                return breedResult.Error.ToErrorsList();

            var petsResult = await _volunteersContract
                .GetFilteredPetsWithPagination(
                new GetFilteredPetsWithPaginationRequest(1, 10, BreedId: command.BreedId), cancellationToken);
            if (petsResult.Value.TotalCount != 0)
                return Errors.General.Conflict().ToErrorsList();

            var speciesResult = await _repository
                .GetById(command.SpeciesId, cancellationToken);

            speciesResult.Value.RemoveBreed(breedResult.Value);
            await _unitOfWork.SaveChanges(cancellationToken);

            return breedResult.Value.Id;
        }
    }
}
