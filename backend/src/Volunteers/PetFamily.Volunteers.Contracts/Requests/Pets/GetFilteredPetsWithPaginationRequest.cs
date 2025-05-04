using PetFamily.Volunteers.Application.Pets.Queries.GetFilteredWithPagiation;
using PetFamily.Volunteers.Domain.PetsVO;

namespace PetFamily.Volunteers.Contracts.Requests.Pets
{
    public record GetFilteredPetsWithPaginationRequest(
        string? Nickname,
        string? Description,
        Guid? SpeciesId,
        Guid? BreedId,
        Guid? VolunteerId,
        string? Color,
        bool? IsSterilized,
        bool? IsVaccinated,
        string? HealthInformation,
        int? Age,
        float? Height,
        float? Weight,
        Status? Status,
        string? PhoneNumber,
        string? City,
        string? Street,
        string? Apartment,
        string? SortBy,
        string? SortDirection,
        int Page,
        int PageSize)
    {
        public GetFilteredPetsWithPaginationQuery ToQuery()
            => new GetFilteredPetsWithPaginationQuery(
                Page,
                PageSize,
                Nickname,
                Description,
                SpeciesId,
                BreedId,
                VolunteerId,
                Color,
                IsSterilized,
                IsVaccinated,
                HealthInformation,
                Age,
                Height,
                Weight,
                Status,
                PhoneNumber,
                City,
                Street,
                Apartment,
                SortBy,
                SortDirection);
    }
}
