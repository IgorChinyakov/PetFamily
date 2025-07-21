using CSharpFunctionalExtensions;
using PetFamily.Discussions.Application.Features.Create;
using PetFamily.Discussions.Contracts;
using PetFamily.Discussions.Contracts.Requests;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Presentation
{
    public class DiscussionsContract : IDiscussionsContract
    {
        private readonly CreateDiscussionHandler _handler;

        public DiscussionsContract(CreateDiscussionHandler handler)
        {
            _handler = handler;
        }

        public async Task<Result<Guid, ErrorsList>> CreateDiscussion(
            CreateDiscussionRequest request, 
            CancellationToken cancellationToken = default)
        {
            var command = new CreateDiscussionCommand(request.RelationId, request.UserIds);

            var result = await _handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error;

            return result.Value;
        }
    }
}
