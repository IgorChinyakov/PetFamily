using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity.Data;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Application.Pets.UploadPhotos
{
    public class UploadPhotosHandler
    {
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IFilesProvider _filesProvider;

        public UploadPhotosHandler(
            IVolunteerRepository volunteerRepository, IFilesProvider filesProvider)
        {
            _volunteerRepository = volunteerRepository;
            _filesProvider = filesProvider;
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
                return uploadResult.Error.ToErrorsList();

            petResult.Value.AddFiles(petFiles);

            await _volunteerRepository.Save(volunteerResult.Value);

            return Result.Success<ErrorsList>();
        }
    }
}
