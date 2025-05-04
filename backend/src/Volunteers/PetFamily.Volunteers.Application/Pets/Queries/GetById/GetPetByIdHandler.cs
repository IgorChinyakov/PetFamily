using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.DTOs;
using PetFamily.Domain.Shared;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Pets.Queries.GetById
{
    public class GetPetByIdHandler : IQueryHandlerWithResult<PetDto, GetPetByIdQuery>
    {
        private readonly ISpeciesReadDbContext _readDbContext;

        public GetPetByIdHandler(ISpeciesReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<Result<PetDto, ErrorsList>> Handle(
            GetPetByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            var petDto = await _readDbContext.Pets
                .FirstOrDefaultAsync(v => v.Id == query.PetId, cancellationToken);

            if (petDto == null)
                return Errors.General.NotFound(query.PetId).ToErrorsList();

            return petDto;
        }
    }
}
