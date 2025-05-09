using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination
{
    public record GetVolunteersWithPaginationQueryWithDapper(int Page, int PageSize) : IQuery;
}
