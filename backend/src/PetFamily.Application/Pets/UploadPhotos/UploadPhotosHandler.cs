using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Messaging;
using PetFamily.Application.Pets.Move;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Application.Pets.UploadPhotos
{
    public class UploadPhotosHandler
    {
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IFilesProvider _filesProvider;
        private readonly ILogger<MovePetHandler> _logger;
        private readonly IMessageQueue<IEnumerable<FileMeta>> _messageQueue;

        public UploadPhotosHandler(
            IVolunteerRepository volunteerRepository, 
            IFilesProvider filesProvider, 
            ILogger<MovePetHandler> logger,
            IMessageQueue<IEnumerable<FileMeta>> messageQueue)
        {
            _volunteerRepository = volunteerRepository;
            _filesProvider = filesProvider;
            _logger = logger;
            _messageQueue = messageQueue;
        }

        public async Task<UnitResult<ErrorsList>> Handle(
            UploadPhotosCommand command, CancellationToken token = default)
        {
            var volunteerResult = await
                _volunteerRepository.GetById(command.VolunteerId, token);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorsList();

            var petResult = await
                _volunteerRepository.GetPetById(command.VolunteerId, command.PetId, token);
            if(petResult.IsFailure)
                return petResult.Error.ToErrorsList();

            List<PetFile> petFiles = [];
            List<FileContent> fileContents = [];
            foreach (var file in command.FileDtos)
            {
                var extension = Path.GetExtension(file.FileName);

                var filePath = Guid.NewGuid() + extension;
                var petFile = PetFile.Create(filePath);
                if(petFile.IsFailure)
                    return petFile.Error.ToErrorsList();

                petFiles.Add(petFile.Value);
                var fileContent = new FileContent(file.Stream, filePath);
                fileContents.Add(fileContent);
            }

            var fileData = new FilesData(fileContents, command.BucketName);

            var uploadResult = await _filesProvider.UploadFiles(fileData, token);
            if(uploadResult.IsFailure)
            {
                //добавить данные о путях в channel
                var fileMetas = fileContents.Select(fc => new FileMeta(command.BucketName, fc.FileName));
                await _messageQueue.WriteAsync(fileMetas);
                return uploadResult.Error.ToErrorsList();
            }

            petResult.Value.AddFiles(petFiles);

            await _volunteerRepository.Save(volunteerResult.Value);

            _logger.LogInformation("Photos for pet with id {petResult} have been uploaded", petResult.Value.Id);

            return Result.Success<ErrorsList>();
        }
    }
}
