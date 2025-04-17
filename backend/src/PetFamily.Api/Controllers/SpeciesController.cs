using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Api.Extensions;
using PetFamily.Api.Requests.Breeds;
using PetFamily.Api.Requests.Species;
using PetFamily.Api.Requests.Volunteers;
using PetFamily.Api.Response;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs;
using PetFamily.Application.EntitiesHandling.Breeds.Commands.Create;
using PetFamily.Application.EntitiesHandling.Breeds.Commands.Delete;
using PetFamily.Application.EntitiesHandling.Breeds.Queries.GetBreedsWithPagination;
using PetFamily.Application.EntitiesHandling.Specieses.Commands.Create;
using PetFamily.Application.EntitiesHandling.Specieses.Commands.Delete;
using PetFamily.Application.EntitiesHandling.Specieses.Queries.GetSpeciesWithPagination;
using PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteerById;
using PetFamily.Application.EntitiesHandling.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Api.Controllers
{
    public class SpeciesController : ApplicationController
    {
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(
            [FromRoute] Guid id,
            [FromServices] ICommandHandler<Guid, DeleteSpeciesCommand> handler)
        {
            var command = new DeleteSpeciesCommand(id);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpDelete("{speciesId:guid}/breeds/{breedId:guid}")]
        public async Task<ActionResult> DeleteBreed(
            [FromRoute] Guid speciesId,
            [FromRoute] Guid breedId,
            [FromServices] ICommandHandler<Guid, DeleteBreedCommand> handler)
        {
            var command = new DeleteBreedCommand(speciesId, breedId);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost]
        public async Task<ActionResult> Create(
            [FromBody] CreateSpeciesRequest request,
            [FromServices] ICommandHandler<Guid,CreateSpeciesCommand> handler)
        {
            var command = request.ToCommand();

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpPost("{speciesId:guid}/breeds")]
        public async Task<ActionResult> CreateBreed(
            [FromBody] CreateBreedRequest request,
            [FromRoute] Guid speciesId,
            [FromServices] ICommandHandler<Guid, CreateBreedCommand> handler)
        {
            var command = request.ToCommand(speciesId);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [HttpGet]
        public async Task<ActionResult<Guid>> Get(
            [FromQuery] GetSpeciesWithPaginationRequest request,
            [FromServices] IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery> handler,
            CancellationToken token = default)
        {
            var query = request.ToQuery();

            var response = await handler.Handle(query, token);

            return Ok(Envelope.Ok(response));
        }

        [HttpGet("{id:guid}/breeds")]
        public async Task<ActionResult<Guid>> GetBreedsBySpeciesId(
            [FromRoute] Guid id,
            [FromQuery] GetBreedsWithPaginationRequest request,
            [FromServices] IQueryHandler<PagedList<BreedDto>, GetBreedsWithPaginationQuery> handler,
            CancellationToken token = default)
        {
            var query = request.ToQuery(id);

            var response = await handler.Handle(query, token);

            return Ok(Envelope.Ok(response));
        }
    }
}
