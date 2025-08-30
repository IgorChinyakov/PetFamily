using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Commands.UpdateDiscussionId
{
    public class UpdateDiscussionIdHandler : ICommandHandler<UpdateDiscussionIdCommand>
    {
        private readonly IVolunteerRequestsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDiscussionIdHandler(
            IVolunteerRequestsRepository repository, 
            [FromKeyedServices(UnitOfWorkKeys.VolunteerRequests)] IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            UpdateDiscussionIdCommand command, 
            CancellationToken cancellationToken = default)
        {
            var requestId = RequestId.Create(command.RequestId);
            var request = await _repository.GetById(requestId);
            if (request.IsFailure)
                return request.Error.ToErrorsList();

            request.Value.UpdateDiscussionId(DiscussionId.Create(command.DiscussionId));

            await _unitOfWork.SaveChanges();

            return Result.Success<ErrorsList>();
        }
    }
}
