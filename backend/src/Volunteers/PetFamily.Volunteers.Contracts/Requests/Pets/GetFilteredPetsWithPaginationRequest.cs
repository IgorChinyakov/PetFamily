using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Contracts.Requests.Pets
{
    public record GetFilteredPetsWithPaginationRequest(
        int Page,
        int PageSize,
        string? Nickname = null,
        string? Description = null,
        Guid? SpeciesId = null,
        Guid? BreedId = null,
        Guid? VolunteerId = null,
        string? Color = null,
        bool? IsSterilized = null,
        bool? IsVaccinated = null,
        string? HealthInformation = null,
        int? Age = null,
        float? Height = null,
        float? Weight = null,
        PetStatusDto? Status = null,
        string? PhoneNumber = null,
        string? City = null,
        string? Street = null,
        string? Apartment = null,
        string? SortBy = null,
        string? SortDirection = null);
}
