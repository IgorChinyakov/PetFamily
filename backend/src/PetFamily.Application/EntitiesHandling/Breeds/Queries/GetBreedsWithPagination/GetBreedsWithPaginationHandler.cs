using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.EntitiesHandling.Breeds.Queries.GetBreedsWithPagination
{
    public class GetBreedsWithPaginationHandler : 
        IQueryHandler<PagedList<BreedDto>, GetBreedsWithPaginationQuery>
    {
        private readonly IReadDbContext _readDbContext;

        public GetBreedsWithPaginationHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<BreedDto>> Handle(
            GetBreedsWithPaginationQuery query, 
            CancellationToken cancellationToken = default)
        {
            var breeds = await _readDbContext.Breeds
                .Where(b => b.SpeciesId == query.SpeciesId)
                .GetWithPagination(query.Page, query.PageSize);

            return breeds;
        }
    }
}
