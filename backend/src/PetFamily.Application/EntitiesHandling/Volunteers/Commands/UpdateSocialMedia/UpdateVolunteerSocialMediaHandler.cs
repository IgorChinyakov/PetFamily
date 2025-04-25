using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Application.EntitiesHandling.Volunteers.Commands.UpdateSocialMedia
{
    public class UpdateVolunteerSocialMediaHandler : 
        ICommandHandler<Guid, UpdateVolunteerSocialMediaCommand>
    {
        private readonly IVolunteerRepository _repository;
        private readonly IValidator<UpdateVolunteerSocialMediaCommand> _validator;
        private readonly ILogger<UpdateVolunteerSocialMediaHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVolunteerSocialMediaHandler(
            IVolunteerRepository repository,
            IValidator<UpdateVolunteerSocialMediaCommand> validator,
            ILogger<UpdateVolunteerSocialMediaHandler> logger,
            IUnitOfWork unitOfWork)
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
