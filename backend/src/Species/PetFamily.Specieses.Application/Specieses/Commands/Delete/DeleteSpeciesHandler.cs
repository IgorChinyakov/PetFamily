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

namespace PetFamily.Specieses.Application.Specieses.Commands.Delete
{
    public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteersContract _volunteersContract;

        public DeleteSpeciesHandler(
            ISpeciesRepository repository,
            [FromKeyedServices(UnitOfWorkKeys.Species)]IUnitOfWork unitOfWork,
            IVolunteersContract volunteersContract)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _volunteersContract = volunteersContract;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            DeleteSpeciesCommand command,
            CancellationToken cancellationToken = default)
        {
            var speciesResult = await _repository.GetById(command.Id, cancellationToken);
            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorsList();

            var petsResult = await _volunteersContract.GetFilteredPetsWithPagination(new GetFilteredPetsWithPaginationRequest(1, 10, SpeciesId: command.Id));
            if (petsResult.Value.TotalCount != 0)
                return Errors.General.Conflict().ToErrorsList();

            _repository.HardDelete(speciesResult.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            return speciesResult.Value.Id;
        }
    }
}