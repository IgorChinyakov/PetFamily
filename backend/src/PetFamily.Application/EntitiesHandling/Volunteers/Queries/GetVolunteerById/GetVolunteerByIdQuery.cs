using PetFamily.Application.Abstractions;

namespace PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteerById
{
    public record GetVolunteerByIdQuery(Guid Id) : IQuery;
}