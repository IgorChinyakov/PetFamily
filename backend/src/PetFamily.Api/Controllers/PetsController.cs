using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Api.Requests.Pets;
using PetFamily.Api.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs;
using PetFamily.Application.EntitiesHandling.Pets.Queries.GetById;
using PetFamily.Application.EntitiesHandling.Pets.Queries.GetFilteredWithPagiation;
using PetFamily.Application.Models;

namespace PetFamily.Api.Controllers
{
    public class PetsController : ApplicationController
    {
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

        [HttpGet]
        public async Task<ActionResult> GetFilteredPetsWithPagination(
            [FromServices] IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery> handler,
            [FromQuery] GetFilteredPetsWithPaginationRequest request,
            CancellationToken token = default)
        {
            var query = request.ToQuery();

            var result = await handler.Handle(query, token);

            return Ok(Envelope.Ok(result));
        }
    }
}
