using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteers.Application.Pets.Queries.GetById;
using PetFamily.Volunteers.Application.Pets.Queries.GetFilteredWithPagiation;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.Requests.Pets;

namespace PetFamily.Volunteers.Presentation.Controllers
{
    public class PetsController : ApplicationController
    {
        [Permission(Permissions.Pet.GET)]
        [HttpGet("{petId:guid}")]
        public async Task<ActionResult> GetPetById(
            [FromServices] IQueryHandlerWithResult<PetDto, GetPetByIdQuery> handler,
            [FromRoute] Guid petId,
            CancellationToken token = default)
        {
            var query = new GetPetByIdQuery(petId);

            var result = await handler.Handle(query, token);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [Permission(Permissions.Pet.GET)]
        [HttpGet]
        public async Task<ActionResult> GetFilteredPetsWithPagination(
            [FromServices] IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery> handler,
            [FromQuery] GetFilteredPetsWithPaginationRequest request,
            CancellationToken token = default)
        {
            var query = new GetFilteredPetsWithPaginationQuery(
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
                request.SortDirection);

            var result = await handler.Handle(query, token);

            return Ok(Envelope.Ok(result));
        }
    }
}
