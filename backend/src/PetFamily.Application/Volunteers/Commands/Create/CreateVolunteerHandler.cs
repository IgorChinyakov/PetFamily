using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Application.Volunteers.Commands.Create
{
    public class CreateVolunteerHandler : ICommandHandler<Guid, CreateVolunteerCommand>
    {
        private readonly IVolunteerRepository _repository;
        private readonly IValidator<CreateVolunteerCommand> _validator;
        private readonly ILogger<CreateVolunteerHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVolunteerHandler(
            IVolunteerRepository repository,
            IValidator<CreateVolunteerCommand> validator,
            ILogger<CreateVolunteerHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorsList>> Handle(
            CreateVolunteerCommand command,
            CancellationToken token = default)
        {
            var result = await _validator.ValidateAsync(command, token);

            if (!result.IsValid)
                return result.ToErrorsList();

            var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber).Value;
            var fullNameResult = FullName.Create(command.FullName.Name,
                command.FullName.SecondName,
                command.FullName.FamilyName).Value;
            var emailResult = Email.Create(command.Email).Value;
            var descriptionResult = Description.Create(command.Description).Value;
            var experienceResult = Experience.Create(command.Experience).Value;

            var detailsList = command.DetailsList.Select(d => Details.Create(d.Title, d.Description).Value).ToList();
            var socialMediaList = command.SocialMediaList.Select(d => SocialMedia.Create(d.Title, d.Link).Value).ToList();

            var volunteerResult = await _repository.GetByPhoneNumber(phoneNumberResult);

            if (volunteerResult.IsSuccess)
                return Errors.General.Conflict().ToErrorsList();

            var volunteer = new Volunteer(
                Guid.NewGuid(),
                fullNameResult,
                emailResult,
                descriptionResult,
                experienceResult,
                phoneNumberResult,
                detailsList,
                socialMediaList);

            await _repository.Add(volunteer, token);
            await _unitOfWork.SaveChanges(token);

            _logger.LogInformation("Volunteer {fullNmaeResult} with id {volunteerId} has been created", fullNameResult, volunteer.Id);

            return volunteer.Id;
        }
    }
}
