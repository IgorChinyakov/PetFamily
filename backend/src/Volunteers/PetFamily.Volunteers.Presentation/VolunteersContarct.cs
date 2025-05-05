using CSharpFunctionalExtensions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Pets.Queries.GetFilteredWithPagiation;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests.Pets;

namespace PetFamily.Volunteers.Presentation
{
    public class VolunteersContarct : IVolunteersContract
    {
        private readonly GetFilteredPetsWithPaginationHandler _getFilteredPetsWithPaginationHandler;

        public VolunteersContarct(
            GetFilteredPetsWithPaginationHandler getFilteredPetsWithPaginationHandler)
        {
            _getFilteredPetsWithPaginationHandler = getFilteredPetsWithPaginationHandler;
        }

        public async Task<Result<PagedList<PetDto>>> GetFilteredPetsWithPagination(
            GetFilteredPetsWithPaginationRequest request, 
            CancellationToken cancellationToken = default)
        {
            return await _getFilteredPetsWithPaginationHandler
                .Handle(new GetFilteredPetsWithPaginationQuery(
                    request.Page, 
                    request.PageSize,
                    request.Nickname,
                    request.Description,
                    request.SpeciesId,
                    request.BreedId,
                    request.VolunteerId,
                    request.Color,
                    request.IsSterilized,
                    request.IsVaccinated,
                    request.HealthInformation,
                    request.Age,
                    request.Height,
                    request.Weight,
                    request.Status,
                    request.PhoneNumber,
                    request.City,
                    request.Street,
                    request.Apartment,
                    request.SortBy,
                    request.SortDirection), cancellationToken);
        }
    }
}
