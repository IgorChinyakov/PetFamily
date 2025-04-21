using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.EntitiesHandling.Pets.Commands.Move;
using PetFamily.Application.EntitiesHandling.Volunteers;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Application.EntitiesHandling.Pets.Commands.UploadPhotos
{
    public class UploadPetPhotosHandler : ICommandHandler<IReadOnlyList<string>, UploadPetPhotosCommand>
    {
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IFilesProvider _filesProvider;
        private readonly ILogger<MovePetHandler> _logger;
        private readonly IMessageQueue<IEnumerable<FileMeta>> _messageQueue;
        private readonly IUnitOfWork _unitOfWork;

        public UploadPetPhotosHandler(
            IVolunteerRepository volunteerRepository,
            IFilesProvider filesProvider,
            ILogger<MovePetHandler> logger,
            IMessageQueue<IEnumerable<FileMeta>> messageQueue,
            IUnitOfWork unitOfWork)
        {
            _volunteerRepository = volunteerRepository;
            _filesProvider = filesProvider;
            _logger = logger;
            _messageQueue = messageQueue;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IReadOnlyList<string>, ErrorsList>> Handle(
            UploadPetPhotosCommand command, CancellationToken token = default)
        {
            var transaction = await _unitOfWork.BeginTransaction(token);

            try
            {
                var volunteerResult = await
                _volunteerRepository.GetById(command.VolunteerId, token);
                if (volunteerResult.IsFailure)
                    return volunteerResult.Error.ToErrorsList();

                var petResult = await
                    _volunteerRepository.GetPetById(command.VolunteerId, command.PetId, token);
                if (petResult.IsFailure)
                    return petResult.Error.ToErrorsList();

                List<PetFile> petFiles = [];
                List<FileData> filesData = [];
                foreach (var file in command.FileDtos)
                {
                    var extension = Path.GetExtension(file.FileName);

                    var filePath = Guid.NewGuid() + extension;
                    var petFile = FilePath.Create(filePath);
                    if (petFile.IsFailure)
                        return petFile.Error.ToErrorsList();

                    petFiles.Add(new PetFile(petFile.Value));
                    var fileData = new FileData(file.Content, filePath, command.BucketName);
                    filesData.Add(fileData);
                }

                var uploadResult = await _filesProvider.UploadFiles(filesData, token);
                if (uploadResult.IsFailure)
                {
                    //добавить данные о путях в channel
                    var fileMetas = filesData.Select(fc => new FileMeta(command.BucketName, fc.FilePath));
                    await _messageQueue.WriteAsync(fileMetas);
                    return uploadResult.Error.ToErrorsList();
                }

                petResult.Value.AddFiles(petFiles);

                await _unitOfWork.SaveChanges(token);

                transaction.Commit();

                _logger.LogInformation("Photos for pet with id {petResult} have been uploaded", petResult.Value.Id);
                
                return uploadResult.Value.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload files to pet with id: {id}", command.PetId);

                transaction.Rollback();

                return Error.Failure("failed.to.upload.files", "Failed to upload files to pet").ToErrorsList();
            }
        }
    }
}
