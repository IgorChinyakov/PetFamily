using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Contracts.DTOs;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Queries.GetDiscussionByRelationId
{
    public class GetDiscussionByRealtionIdHandler : 
        IQueryHandlerWithResult<DiscussionDto, GetDiscussionByRelationIdQuery>
    {
        private readonly IDiscussionsReadDbContext _readDbContext;

        public GetDiscussionByRealtionIdHandler(IDiscussionsReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public async Task<Result<DiscussionDto, ErrorsList>> Handle(
            GetDiscussionByRelationIdQuery query, 
            CancellationToken cancellationToken = default)
        {
            var discussionDto = await _readDbContext.Discussions
                .Include(d => d.MessageDtos)
                .FirstOrDefaultAsync(
                d => d.RelationId == query.RelationId, cancellationToken);

            if (discussionDto == null)
                return Errors.General.NotFound(query.RelationId).ToErrorsList();

            return discussionDto;
        }
    }
}
