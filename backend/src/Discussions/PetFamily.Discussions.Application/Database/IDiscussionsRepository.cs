using CSharpFunctionalExtensions;
using PetFamily.Discussions.Domain.Entities;
using PetFamily.Discussions.Domain.ValueObjects.Discussion;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Database
{
    public interface IDiscussionsRepository
    {
        Task<Guid> Add(Discussion discussion, CancellationToken token = default);
        Task<Result<Discussion, Error>> GetByRelationId(RelationId relationId);
    }
}
