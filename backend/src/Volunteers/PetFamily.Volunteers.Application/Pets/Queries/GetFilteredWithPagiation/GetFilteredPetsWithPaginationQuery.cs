using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Domain.PetsVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetFamily.Volunteers.Domain.PetsVO.PetStatus;

namespace PetFamily.Volunteers.Application.Pets.Queries.GetFilteredWithPagiation
{
    public record GetFilteredPetsWithPaginationQuery(
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
        string? SortDirection = null) : IQuery;
}
