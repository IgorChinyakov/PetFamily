using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteerById
{
    public class GetVolunteerByIdHandler :
        IQueryHandlerWithResult<VolunteerDto, GetVolunteerByIdQuery>
    {
        private readonly IVolunteersReadDbContext _readDbContext;

        public GetVolunteerByIdHandler(IVolunteersReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<Result<VolunteerDto, ErrorsList>> Handle(
            GetVolunteerByIdQuery query, CancellationToken cancellationToken = default)
        {
            var volunteerDto = await _readDbContext.Volunteers
                .FirstOrDefaultAsync(v => v.Id == query.Id, cancellationToken);

            if (volunteerDto == null)
                return Errors.General.NotFound(query.Id).ToErrorsList();

            return volunteerDto;
        }
    }
}
