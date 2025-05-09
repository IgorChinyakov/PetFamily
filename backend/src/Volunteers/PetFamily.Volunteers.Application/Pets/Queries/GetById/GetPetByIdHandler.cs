using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Application.Pets.Queries.GetById
{
    public class GetPetByIdHandler : IQueryHandlerWithResult<PetDto, GetPetByIdQuery>
    {
        private readonly IVolunteersReadDbContext _readDbContext;

        public GetPetByIdHandler(IVolunteersReadDbContext readDbContext)
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
