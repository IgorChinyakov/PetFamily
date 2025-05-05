using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.DTOs;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Application.Specieses.Queries.GetSpeciesById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Specieses.Application.Breeds.Queries.GetBreedById
{
    public class GetBreedByIdHandler :
        IQueryHandlerWithResult<BreedDto, GetBreedByIdQuery>
    {
        private readonly ISpeciesReadDbContext _readDbContext;

        public GetBreedByIdHandler(ISpeciesReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<Result<BreedDto, ErrorsList>> Handle(
            GetBreedByIdQuery query, 
            CancellationToken cancellationToken = default)
        {
            var breedDto = await _readDbContext.Breeds
                .FirstOrDefaultAsync(b => b.SpeciesId == query.SpeciesId && b.Id == query.BreedId, cancellationToken);
            if (breedDto == null)
                return Errors.General.NotFound(query.BreedId).ToErrorsList();

            return breedDto;
        }
    }
}
