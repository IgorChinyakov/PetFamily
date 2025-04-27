using PetFamily.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Application.EntitiesHandling.Specieses.Queries.GetSpeciesWithPagination
{
    public record GetSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery
    {
        public GetSpeciesWithPaginationQuery ToQuery()
            => new GetSpeciesWithPaginationQuery(Page, PageSize);
    }
}
