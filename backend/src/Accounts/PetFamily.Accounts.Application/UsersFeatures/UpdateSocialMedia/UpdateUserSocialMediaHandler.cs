using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.Entities;
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

namespace PetFamily.Accounts.Application.Features.UpdateSocialMedia
{
    public class UpdateUserSocialMediaHandler
        : ICommandHandler<Guid, UpdateUserSocialMediaCommand>
    {
        private readonly IValidator<UpdateUserSocialMediaCommand> _validator;
        private readonly ILogger<UpdateUserSocialMediaHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public UpdateUserSocialMediaHandler(
            IValidator<UpdateUserSocialMediaCommand> validator,
            ILogger<UpdateUserSocialMediaHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Accounts)] IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            UpdateUserSocialMediaCommand command, 
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var user = await _userManager.FindByIdAsync(command.Id.ToString());
            if (user == null)
                return Errors.General.ValueIsInvalid("userId").ToErrorsList();

            var socialMedia = command.SocialMedia.Select(sm => SocialMedia.Create(sm.Title, sm.Link).Value);
            user.UpdateSocialMedia(socialMedia.ToList());

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("User's social media list has been updated. user Id: {userId}", user.Id);

            return user.Id;
        }
    }
}
