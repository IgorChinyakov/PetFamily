using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Specieses.Application.Breeds.Commands.Create;
using PetFamily.Specieses.Application.Breeds.Commands.Delete;
using PetFamily.Specieses.Application.Breeds.Queries.GetBreedsWithPagination;
using PetFamily.Specieses.Application.Specieses.Commands.Create;
using PetFamily.Specieses.Application.Specieses.Commands.Delete;
using PetFamily.Specieses.Application.Specieses.Queries.GetSpeciesWithPagination;
using PetFamily.Specieses.Contracts.DTOs;
using PetFamily.Specieses.Contracts.Requests.Breeds;
using PetFamily.Specieses.Contracts.Requests.Species;

namespace PetFamily.Specieses.Presentation.Controllers
{
    public class SpeciesController : ApplicationController
    {
        [Permission(Permissions.Species.DELETE)]
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

        [Permission(Permissions.Breeds.DELETE)]
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

        [Permission(Permissions.Species.CREATE)]
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromBody] CreateSpeciesRequest request,
            [FromServices] ICommandHandler<Guid, CreateSpeciesCommand> handler)
        {
            var command = new CreateSpeciesCommand(request.Name);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [Permission(Permissions.Breeds.CREATE)]
        [HttpPost("{speciesId:guid}/breeds")]
        public async Task<ActionResult> CreateBreed(
            [FromBody] CreateBreedRequest request,
            [FromRoute] Guid speciesId,
            [FromServices] ICommandHandler<Guid, CreateBreedCommand> handler)
        {
            var command = new CreateBreedCommand(speciesId, request.Name);

            var result = await handler.Handle(command);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(Envelope.Ok(result.Value));
        }

        [Permission(Permissions.Species.GET)]
        [HttpGet]
        public async Task<ActionResult<Guid>> Get(
            [FromQuery] GetSpeciesWithPaginationRequest request,
            [FromServices] IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery> handler,
            CancellationToken token = default)
        {
            var query = new GetSpeciesWithPaginationQuery(request.Page, request.PageSize);

            var response = await handler.Handle(query, token);

            return Ok(Envelope.Ok(response));
        }

        [Permission(Permissions.Breeds.GET)]
        [HttpGet("{id:guid}/breeds")]
        public async Task<ActionResult> GetBreedsBySpeciesId(
            [FromRoute] Guid id,
            [FromQuery] GetBreedsWithPaginationRequest request,
            [FromServices] IQueryHandler<PagedList<BreedDto>, GetBreedsWithPaginationQuery> handler,
            CancellationToken token = default)
        {
            var query = new GetBreedsWithPaginationQuery(id, request.Page, request.PageSize);

            var response = await handler.Handle(query, token);

            return Ok(Envelope.Ok(response));
        }
    }
}
