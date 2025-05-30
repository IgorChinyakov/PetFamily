﻿using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.Database;
using PetFamily.Core.Options;
using PetFamily.Files.Contracts;
using PetFamily.Files.Contracts.DTOs;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Application.Pets.Commands.Move;
using PetFamily.Volunteers.Domain.PetsVO;

namespace PetFamily.Volunteers.Application.Pets.Commands.UploadPhotos
{
    public class UploadPetPhotosHandler :
        ICommandHandler<IReadOnlyList<string>, UploadPetPhotosCommand>
    {
        private readonly IVolunteersRepository _volunteerRepository;
        private readonly IFilesContract _filesContract;
        private readonly ILogger<MovePetHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UploadPetPhotosHandler(
            IVolunteersRepository volunteerRepository,
            IFilesContract filesContract,
            ILogger<MovePetHandler> logger,
            [FromKeyedServices(UnitOfWorkKeys.Volunteers)] IUnitOfWork unitOfWork)
        {
            _volunteerRepository = volunteerRepository;
            _filesContract = filesContract;
            _logger = logger;
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

                var uploadResult = await _filesContract.UploadFiles(filesData, token);
                if (uploadResult.IsFailure)
                {
                    //добавить данные о путях в channel
                    var fileMetas = filesData.Select(fc => new FileMeta(command.BucketName, fc.FilePath));
                    await _filesContract.AddToMessageQueue(fileMetas);
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
