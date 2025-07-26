using CSharpFunctionalExtensions;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Contracts
{
    public interface IDiscussionsContract
    {
        public Task<Result<Guid, ErrorsList>> CreateDiscussion(
            CreateDiscussionRequest request,
            CancellationToken cancellationToken = default);
    }
}
