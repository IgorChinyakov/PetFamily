using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Features.Commands.SendForRevision
{
    public class SendRequestForRevisionHandler :
        ICommandHandler<SendRequestForRevisionCommand>
    {
        private readonly IVolunteerRequestsRepository _repository;
        private readonly IValidator<SendRequestForRevisionCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;

        public SendRequestForRevisionHandler(
            IVolunteerRequestsRepository repository,
            IValidator<SendRequestForRevisionCommand> validator,
            [FromKeyedServices(UnitOfWorkKeys.VolunteerRequests)] IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            SendRequestForRevisionCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var requestId = RequestId.Create(command.RequestId);
            var request = await _repository.GetById(requestId);
            if (request.IsFailure)
                return request.Error.ToErrorsList();

            if (request.Value.AdminId != AdminId.Create(command.AdminId))
                return Error.AccessDenied(
                    "request.by.another.admin",
                    "Can't send request for revision which is on review by another admin").ToErrorsList();

            var rejectionComment = RejectionComment.Create(command.RejectionComment).Value;
            var revisionResult = request.Value.SendForRevision(rejectionComment);
            if (revisionResult.IsFailure)
                revisionResult.Error.ToErrorsList();

            await _unitOfWork.SaveChanges();

            return Result.Success<ErrorsList>();
        }
    }
}
