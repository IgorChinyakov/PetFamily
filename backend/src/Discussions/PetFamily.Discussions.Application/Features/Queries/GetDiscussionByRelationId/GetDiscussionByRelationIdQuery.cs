using PetFamily.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Application.Features.Queries.GetDiscussionByRelationId
{
    public record GetDiscussionByRelationIdQuery(Guid RelationId) : IQuery;
}
