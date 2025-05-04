using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Extensions;
using PetFamily.Core.Options;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.VolunteersVO;

namespace PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialMedia
{
    public class UpdateVolunteerSocialMediaHandler :
        ICommandHandler<Guid, UpdateVolunteerSocialMediaCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<UpdateVolunteerSocialMediaCommand> _validator;
        private readonly ILogger<UpdateVolunteerSocialMediaHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVolunteerSocialMediaHandler(
            IVolunteersRepository repository,
            IValidator<UpdateVolunteerSocialMediaCommand> validator,
            ILogger<UpdateVolunteerSocialMediaHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Volunteers)] IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            UpdateVolunteerSocialMediaCommand command,
            CancellationToken token = default)
        {
            var validationResult = await _validator.ValidateAsync(command, token);

            if (!validationResult.IsValid)
                return validationResult.ToErrorsList();

            var volunteerResult = await _repository.GetById(command.Id);
            if (volunteerResult.IsFailure)
                return Errors.General.ValueIsInvalid("VolunteerId").ToErrorsList();

            var socialMedia = command.SocialMedia.Select(sm => SocialMedia.Create(sm.Title, sm.Link).Value);
            volunteerResult.Value.UpdateSocialMediaList(socialMedia);

            await _unitOfWork.SaveChanges(token);

            _logger.LogInformation("Volunteer's main info has been updated. Volunteer Id: {volunteerId}", volunteerResult.Value.Id);

            return volunteerResult.Value.Id;
        }
    }
}
