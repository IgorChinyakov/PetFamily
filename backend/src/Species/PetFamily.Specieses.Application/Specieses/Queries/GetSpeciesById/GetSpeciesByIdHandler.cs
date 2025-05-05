using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.DTOs;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Specieses.Application.Specieses.Queries.GetSpeciesById
{
    public class GetSpeciesByIdHandler :
        IQueryHandlerWithResult<SpeciesDto, GetSpeciesByIdQuery>
    {
        private readonly ISpeciesReadDbContext _readDbContext;

        public GetSpeciesByIdHandler(ISpeciesReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<Result<SpeciesDto, ErrorsList>> Handle(
            GetSpeciesByIdQuery query, CancellationToken cancellationToken = default)
        {
            var speciesDto = await _readDbContext.Species
                .FirstOrDefaultAsync(v => v.Id == query.Id, cancellationToken);

            if (speciesDto == null)
                return Errors.General.NotFound(query.Id).ToErrorsList();

            return speciesDto;
        }
    }
}
