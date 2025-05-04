using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared;
using PetFamily.Specieses.Application.Specieses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Specieses.Application.Breeds.Commands.Delete
{
    public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReadDbContext _readDbContext;

        public DeleteBreedHandler(
            ISpeciesRepository repository,
            IUnitOfWork unitOfWork,
            IReadDbContext readDbContext)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _readDbContext = readDbContext;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            DeleteBreedCommand command,
            CancellationToken cancellationToken = default)
        {
            var breedResult = await _repository
                .GetBreedById(command.SpeciesId, command.BreedId, cancellationToken);
            if (breedResult.IsFailure)
                return breedResult.Error.ToErrorsList();

            var petsResult = await _readDbContext.Pets
                .FirstOrDefaultAsync(p => p.BreedId == command.BreedId);
            if (petsResult != null)
                return Errors.General.Conflict().ToErrorsList();

            var speciesResult = await _repository
                .GetById(command.SpeciesId, cancellationToken);

            speciesResult.Value.RemoveBreed(breedResult.Value);
            await _unitOfWork.SaveChanges(cancellationToken);

            return breedResult.Value.Id;
        }
    }
}
