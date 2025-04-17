using PetFamily.Application.Abstractions;

namespace PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteersWithPagination
{
    public record GetVolunteersWithPaginationQuery(int Page, int PageSize) : IQuery;
}
