using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.DTOs;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using System.Linq.Expressions;

namespace PetFamily.Volunteers.Application.Pets.Queries.GetFilteredWithPagiation
{
    public class GetFilteredPetsWithPaginationHandler :
        IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
    {
        private readonly ISpeciesReadDbContext _readDbContext;

        public GetFilteredPetsWithPaginationHandler(
            ISpeciesReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<PagedList<PetDto>> Handle(
            GetFilteredPetsWithPaginationQuery query,
            CancellationToken cancellationToken = default)
        {
            var petsQuery = _readDbContext.Pets;

            Expression<Func<PetDto, object>> keySelector = query.SortBy?.ToLower() switch
            {
                "nickname" => (p) => p.NickName,
                "breed" => (p) => p.BreedId,
                "species" => (p) => p.SpeciesId,
                "color" => (p) => p.Color,
                "volunteer" => (p) => p.VolunteerId,
                "age" => (p) => p.Birthday,
                "address" => (p) => p.Address.City + p.Address.Street + p.Address.Apartment,
                _ => (p) => p.Id
            };

            if (query.SortBy?.ToLower() == "desc")
                petsQuery = petsQuery.OrderByDescending(keySelector);
            else
                petsQuery = petsQuery.OrderBy(keySelector);

            petsQuery = petsQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.Nickname),
                p => p.NickName.Contains(query.Nickname!));

            petsQuery = petsQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.Description),
                p => p.Description.Contains(query.Description!));

            petsQuery = petsQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.HealthInformation),
                p => p.HealthInformation.Contains(query.HealthInformation!));

            petsQuery = petsQuery.WhereIf(
                query.Height != null,
                p => p.Height == query.Height!);

            petsQuery = petsQuery.WhereIf(
                query.Weight != null,
                p => p.Weight == query.Weight!);

            petsQuery = petsQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.City),
                p => p.Address.City.Contains(query.City!));

            petsQuery = petsQuery.WhereIf(
                query.BreedId != null,
                p => p.BreedId == query.BreedId);

            petsQuery = petsQuery.WhereIf(
                query.SpeciesId != null,
                p => p.SpeciesId == query.SpeciesId);

            petsQuery = petsQuery.WhereIf(
                query.VolunteerId != null,
                p => p.VolunteerId == query.VolunteerId);

            petsQuery = petsQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.Street),
                p => p.Address.Street.Contains(query.Street!));

            petsQuery = petsQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.Apartment),
                p => p.Address.Apartment.Contains(query.Apartment!));

            petsQuery = petsQuery.WhereIf(
                query.IsSterilized != null,
                p => p.IsSterilized == query.IsSterilized);

            petsQuery = petsQuery.WhereIf(
                query.IsVaccinated != null,
                p => p.IsVaccinated == query.IsVaccinated);

            petsQuery = petsQuery.WhereIf(
                query.Status != null,
                p => p.Status == query.Status);

            petsQuery = petsQuery.WhereIf(
                query.Age != null,
                p => (DateTime.UtcNow - p.Birthday).Days / 365 == query.Age);

            petsQuery = petsQuery.WhereIf(
                !string.IsNullOrWhiteSpace(query.PhoneNumber),
                p => p.PhoneNumber.Contains(query.PhoneNumber!));

            return await petsQuery
                .GetWithPagination(query.Page, query.PageSize, cancellationToken);
        }
    }
}
