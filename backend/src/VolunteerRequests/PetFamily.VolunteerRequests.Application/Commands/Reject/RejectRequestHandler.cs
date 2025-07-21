using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Application.Commands.SendForRevision;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Commands.Reject
{
    public class RejectRequestHandler : ICommandHandler<RejectRequestCommand>
    {
        private readonly IVolunteerRequestsRepository _repository;
        private readonly IValidator<RejectRequestCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;

        public RejectRequestHandler(
            IVolunteerRequestsRepository repository,
            IValidator<RejectRequestCommand> validator,
            [FromKeyedServices(UnitOfWorkKeys.VolunteerRequests)] IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            RejectRequestCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var requestId = RequestId.Create(command.RequestId);
            var request = await _repository.GetById(requestId);
            if (request.IsFailure)
                return request.Error.ToErrorsList();

            if (request.Value.AdminId != AdminId.Create(command.AdminId))
                return Error.AccessDenied(
                    "request.by.another.admin", 
                    "Can't reject request which is on review by another admin").ToErrorsList();

            request.Value.Reject();

            await _unitOfWork.SaveChanges();

            return Result.Success<ErrorsList>();
        }
    }
}
