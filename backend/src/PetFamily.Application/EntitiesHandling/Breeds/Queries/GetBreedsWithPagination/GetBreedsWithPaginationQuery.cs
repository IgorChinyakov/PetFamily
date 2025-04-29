using PetFamily.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.EntitiesHandling.Breeds.Queries.GetBreedsWithPagination
{
    public record GetBreedsWithPaginationQuery(
        Guid SpeciesId, int Page, int PageSize) : IQuery;
}
