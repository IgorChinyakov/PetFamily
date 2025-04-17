using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.EntitiesHandling.Specieses.Queries.GetSpeciesWithPagination
{
    public class GetSpeciesWithPaginationHandler : 
        IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetSpeciesWithPaginationHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<SpeciesDto>> Handle(
            GetSpeciesWithPaginationQuery query, 
            CancellationToken cancellationToken = default)
        {
            var speciesQuery = _readDbContext.Species;

            var pagedList = await speciesQuery
                .GetWithPagination(query.Page, query.PageSize, cancellationToken);

            return pagedList;
        }
    }
}
