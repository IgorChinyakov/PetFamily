using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteerById
{
    public class GetVolunteerByIdHandler : IQueryHandler<Result<VolunteerDto, ErrorsList>, GetVolunteerByIdQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetVolunteerByIdHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<Result<VolunteerDto, ErrorsList>> Handle(
            GetVolunteerByIdQuery query, CancellationToken cancellationToken = default)
        {
            var volunteerDto = await _readDbContext.Volunteers
                .FirstOrDefaultAsync(v => v.Id == query.Id, cancellationToken);

            if(volunteerDto == null)
                return Errors.General.NotFound(query.Id).ToErrorsList();            
            
            return volunteerDto;
        }
    }
}
