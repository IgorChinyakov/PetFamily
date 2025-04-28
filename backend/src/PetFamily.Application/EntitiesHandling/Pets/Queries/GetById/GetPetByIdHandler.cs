using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.EntitiesHandling.Pets.Queries.GetById
{
    public class GetPetByIdHandler : IQueryHandlerWithResult<PetDto, GetPetByIdQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetPetByIdHandler(IReadDbContext readDbContext)
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
