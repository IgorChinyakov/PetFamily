using CSharpFunctionalExtensions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application.Breeds.Queries.GetBreedById;
using PetFamily.Specieses.Application.Specieses.Queries.GetSpeciesById;
using PetFamily.Specieses.Application.Specieses.Queries.GetSpeciesWithPagination;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Contracts.Requests.Species;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Specieses.Presentation
{
    public class SpeciesContract : ISpeciesContract
    {
        private readonly GetSpeciesByIdHandler _getSpeciesByIdHandler;
        private readonly GetBreedByIdHandler _getBreedByIdHandler;

        public SpeciesContract(
            GetSpeciesByIdHandler getSpeciesByIdHandler, 
            GetBreedByIdHandler getBreedByIdHandler)
        {
            _getSpeciesByIdHandler = getSpeciesByIdHandler;
            _getBreedByIdHandler = getBreedByIdHandler;
        }

        public async Task<Result<SpeciesDto, ErrorsList>> GetSpeciesById(
            Guid id, CancellationToken cancellationToken = default)
        {
            return await _getSpeciesByIdHandler
                .Handle(new GetSpeciesByIdQuery(id), cancellationToken);
        }

        public async Task<Result<BreedDto, ErrorsList>> GetBreedById(
            Guid speciesId, Guid breedId, CancellationToken cancellationToken = default)
        {
            return await _getBreedByIdHandler
                .Handle(new GetBreedByIdQuery(speciesId, breedId), cancellationToken);
        }
    }
}
