using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Domain.Entities;
using PetFamily.Discussions.Domain.ValueObjects.Discussion;
using PetFamily.Discussions.Infrastructure.DbContexts;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Infrastructure.Repositories
{
    public class DiscussionsRepository : IDiscussionsRepository
    {
        private readonly DiscussionsDbContext _dbContext;

        public DiscussionsRepository(DiscussionsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Add(Discussion discussion, CancellationToken token = default)
        {
            await _dbContext.Discussions.AddAsync(discussion, token);

            return discussion.Id.Value;
        }

        public async Task<Result<Discussion, Error>> GetByRelationId(RelationId relationId)
        {
            var discussionResult = await _dbContext.Discussions
                .FirstOrDefaultAsync(v => v.RelationId == relationId);

            if (discussionResult == null)
                return Errors.General.NotFound();

            return discussionResult;
        }
    }
}
