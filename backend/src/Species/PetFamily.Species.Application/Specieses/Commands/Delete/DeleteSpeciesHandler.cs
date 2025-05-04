using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application.Specieses;

namespace PetFamily.Specieses.Application.Specieses.Commands.Delete
{
    public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteersReadDbContext _readDbContext;

        public DeleteSpeciesHandler(
            ISpeciesRepository repository,
            IUnitOfWork unitOfWork,
            IVolunteersReadDbContext readDbContext)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _readDbContext = readDbContext;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            DeleteSpeciesCommand command,
            CancellationToken cancellationToken = default)
        {
            var speciesResult = await _repository.GetById(command.Id, cancellationToken);
            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorsList();

            var petsResult = await _readDbContext.Pets
                .FirstOrDefaultAsync(p => p.SpeciesId == command.Id);
            if (petsResult != null)
                return Errors.General.Conflict().ToErrorsList();

            _repository.HardDelete(speciesResult.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            return speciesResult.Value.Id;
        }
    }
}