using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Application.Features.UpdateDetails
{
    public class UpdateUserDetailsHandler : ICommandHandler<Guid, UpdateUserDetailsCommand>
    {
        private readonly IVolunteerAccountManager _volunteerAccountManager;
        private readonly IValidator<UpdateUserDetailsCommand> _validator;
        private readonly ILogger<UpdateUserDetailsHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserDetailsHandler(
            IVolunteerAccountManager volunteerAccountManager,
            IValidator<UpdateUserDetailsCommand> validator,
            ILogger<UpdateUserDetailsHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Accounts)] IUnitOfWork unitOfWork)
        {
            _volunteerAccountManager = volunteerAccountManager;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            UpdateUserDetailsCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var volunteerAccount = await _volunteerAccountManager.FindAccountByUserId(command.Id);
            if (volunteerAccount == null)
                return Errors.General.ValueIsInvalid("UserId").ToErrorsList();

            var details = command.Details.Select(sm => Details.Create(sm.Title, sm.Description).Value);
            volunteerAccount.UpdateDetails(details.ToList());
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Volunteer account's details list has been updated. User Id: {userId}", volunteerAccount.UserId);

            return volunteerAccount.UserId;
        }
    }
}
